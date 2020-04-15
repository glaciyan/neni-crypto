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
        public ConcurrentBag<UserDataHeader> ItemHeaders { get; } = new ConcurrentBag<UserDataHeader>();
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
            var itemHeader = UserDataHeader.Create(name, path);
            var destinationPath = Path.Combine(EncryptedFolderPath, itemHeader.TargetPath);

            CheckIfPlainNameAlreadyExists(itemHeader);

            await WriteDecrypted(itemHeader, sourcePath, destinationPath);

            ItemHeaders.Add(itemHeader);
        }

        private async Task WriteDecrypted(UserDataHeader userDataHeader, string sourcePath, string destinationPath)
        {
            var hash = await UserDataFile.WriteUserDataFile(sourcePath, destinationPath,
                Header.MasterPassword.Password, userDataHeader.TargetCipherIV);

            userDataHeader.TargetAuthentication = hash;
        }

        private void CheckIfPlainNameAlreadyExists(UserDataHeader userDataHeader)
        {
            foreach (var header in ItemHeaders)
                if (header.SecuredPlainName.PlainName == userDataHeader.SecuredPlainName.PlainName)
                    throw new FileAlreadyExistsException("File is already in the vault");
        }

        public async Task RemoveFile(UserDataHeader header)
        {
            File.Delete(Path.Combine(EncryptedFolderPath, header.TargetPath));
            if (header.IsUnlocked) await EliminateExtracted(header);
            ItemHeaders.TryTake(out header);
        }

        public async Task MoveFile(UserDataHeader header, string destination)
        {
            var wasUnlocked = header.IsUnlocked;

            if (wasUnlocked) await EliminateExtracted(header);

            header.Move(destination);

            if (wasUnlocked) await ExtractFile(header);
        }

        public async Task UpdateFileContent(UserDataHeader header, string source)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExtractFile(UserDataHeader header)
        {
            var encryptedSourcePath = Path.Combine(EncryptedFolderPath, header.TargetPath);
            var unlockedTarget = Path.Combine(UnlockedFolderPath, header.SecuredPlainName.PlainName);

            var hash = await UserDataFile.ExtractUserDataFile(encryptedSourcePath, unlockedTarget,
                Header.MasterPassword.Password, header.TargetCipherIV);

            header.IsUnlocked = true;

            return hash.ContentEqualTo(header.TargetAuthentication);
        }

        public async Task EliminateExtracted(UserDataHeader header)
        {
            if (!header.IsUnlocked) throw new FileNotUnlockedException();
            var plainTextPath = header.SecuredPlainName.PlainName;

            var path = Path.Combine(UnlockedFolderPath, plainTextPath);

            // TODO: try to search for the file in Unlocked and if found move it (careful with isUnlocked bool)
            if (!File.Exists(path))
            {
                header.IsUnlocked = false;
                throw new FileNotFoundException("Decrypted file was not found", plainTextPath);
            }

            await NFile.Purge(path);
            header.IsUnlocked = false;

            var parentDir = Path.Combine(UnlockedFolderPath, NDirectory.GetPathParentDir(plainTextPath));
            NDirectory.DeleteDirIfEmpty(parentDir, UnlockedFolderName);
        }

        public void CheckAndCorrectAllItemHeaders()
        {
            foreach (var itemHeader in ItemHeaders) CorrectItemHeaderForUnlockedFile(itemHeader);
        }

        public void CorrectItemHeaderForUnlockedFile(UserDataHeader header)
        {
            if (ItemHeaderMissingUnlockedFile(header)) header.IsUnlocked = false;
        }

        private bool ItemHeaderMissingUnlockedFile(UserDataHeader header)
        {
            if (!header.IsUnlocked) return true;

            return !File.Exists(Path.Combine(UnlockedFolderPath, header.SecuredPlainName.PlainName));
        }
    }
}