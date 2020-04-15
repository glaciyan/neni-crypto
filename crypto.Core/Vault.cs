using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using crypto.Core.Exceptions;
using crypto.Core.Extension;
using crypto.Core.Header;

namespace crypto.Core
{
    public class Vault : IDisposable
    {
        private const string FileExtension = ".vlt";
        private const string UnlockedFolderName = "Unlocked";
        private const string EncryptedFolderName = "Encrypted";
        private readonly byte[] _key;

        internal Vault(string name, byte[] key)
        {
            Name = name;
            _key = key;
        }

        public bool Written { get; internal set; }

        private string Name { get; }
        public VaultHeader Header { get; set; }
        public ConcurrentBag<UserDataFile> DataFiles { get; } = new ConcurrentBag<UserDataFile>();
        public string VaultPath { get; set; }
        public string EncryptedFolderPath => Path.Combine(VaultPath, EncryptedFolderName);
        public string UnlockedFolderPath => Path.Combine(VaultPath, UnlockedFolderName);
        public string VaultFilePath => GetVaultFilePath(VaultPath, Name);

        public void Dispose()
        {
            if (!Written)
                VaultReaderWriter.WriteConfig(this, _key);
        }

        internal static string GetVaultFilePath(string vaultPath, string name)
        {
            return vaultPath + "/" + name + FileExtension;
        }

        public static Vault Create(string name, byte[] key, string path = null)
        {
            var output = new Vault(name, key)
            {
                Header = VaultHeader.Create(),
                VaultPath = path == null
                    ? Path.Combine(Environment.CurrentDirectory, name)
                    : Path.GetFullPath(path + "/" + name)
            };

            PrepareVault(output);

            return output;
        }

        public static Vault Open(VaultReadingPaths folderPath, byte[] key)
        {
            return VaultReaderWriter.ReadFromConfig(folderPath, key);
        }

        private static void PrepareVault(Vault vaultFile)
        {
            Directory.CreateDirectory(vaultFile.VaultPath);
            Directory.CreateDirectory(vaultFile.EncryptedFolderPath);
            Directory.CreateDirectory(vaultFile.UnlockedFolderPath);
            File.Create(vaultFile.VaultFilePath).Dispose();
        }

        public async Task AddFileAsync(string sourcePath, string path = "")
        {
            if (!File.Exists(sourcePath)) throw new FileNotFoundException("File not found", sourcePath);

            var name = Path.GetFileName(sourcePath);
            var newFile = new UserDataFile(UserDataHeader.Create(name, path));

            CheckIfPlainNameAlreadyExists(newFile);

            var destinationPath = Path.Combine(EncryptedFolderPath, newFile.Header.TargetPath);
            await WriteDecrypted(newFile, sourcePath, destinationPath);

            DataFiles.Add(newFile);
        }

        private async Task WriteDecrypted(UserDataFile file, string sourcePath, string destinationPath)
        {
            var hash = await UserDataFile.WriteUserDataFile(sourcePath, destinationPath,
                Header.MasterPassword.Password, file.Header.TargetCipherIV);

            file.Header.TargetAuthentication = hash;
        }

        private void CheckIfPlainNameAlreadyExists(UserDataFile file)
        {
            foreach (var vltFile in DataFiles)
                if (vltFile.Header.SecuredPlainName.PlainName == file.Header.SecuredPlainName.PlainName)
                    throw new FileAlreadyExistsException("File is already in the vault");
        }

        public async Task RemoveFile(UserDataFile file)
        {
            File.Delete(Path.Combine(EncryptedFolderPath, file.Header.TargetPath));
            if (file.Header.IsUnlocked) await EliminateExtracted(file);
            DataFiles.TryTake(out file);
        }

        public void MoveFile(UserDataFile file, string destination)
        {
            if (file.Header.IsUnlocked) 

            file.Move(destination);
        }

        public async Task UpdateFileContent(UserDataFile header, string source)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExtractFile(UserDataFile file)
        {
            var encryptedSourcePath = Path.Combine(EncryptedFolderPath, file.Header.TargetPath);
            var unlockedTarget = Path.Combine(UnlockedFolderPath, file.Header.SecuredPlainName.PlainName);

            var hash = await UserDataFile.ExtractUserDataFile(encryptedSourcePath, unlockedTarget,
                Header.MasterPassword.Password, file.Header.TargetCipherIV);

            file.Header.IsUnlocked = true;

            return hash.ContentEqualTo(file.Header.TargetAuthentication);
        }

        public async Task EliminateExtracted(UserDataFile file)
        {
            if (!file.Header.IsUnlocked) throw new FileNotUnlockedException();
            var plainTextPath = file.Header.SecuredPlainName.PlainName;

            var path = Path.Combine(UnlockedFolderPath, plainTextPath);

            // TODO: try to search for the file in Unlocked and if found move it (careful with isUnlocked bool)
            if (!File.Exists(path))
            {
                file.Header.IsUnlocked = false;
                throw new FileNotFoundException("Decrypted file was not found", plainTextPath);
            }

            await NFile.Purge(path);
            file.Header.IsUnlocked = false;

            var parentDir = Path.Combine(UnlockedFolderPath, NDirectory.GetPathParentDir(plainTextPath));
            NDirectory.DeleteDirIfEmpty(parentDir, UnlockedFolderName);
        }

        public void CheckAndCorrectAllItemHeaders()
        {
            foreach (var itemHeader in DataFiles) FixItemHeaderForUnlockedFile(itemHeader);
        }

        private void FixItemHeaderForUnlockedFile(UserDataFile file)
        {
            if (ItemHeaderIsMissingUnlockedFile(file)) file.Header.IsUnlocked = false;
        }

        private bool ItemHeaderIsMissingUnlockedFile(UserDataFile item)
        {
            if (!item.Header.IsUnlocked) return true;

            return !File.Exists(Path.Combine(UnlockedFolderPath, item.Header.SecuredPlainName.PlainName));
        }
    }
}