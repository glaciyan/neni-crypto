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
        private byte[] _key;
        public bool Written { get; internal set; }
        private const string FileExtension = ".vlt";
        private const string UnlockedFolderName = "Unlocked";
        private const string EncryptedFolderName = "Encrypted";

        internal Vault(string name, byte[] key)
        {
            Name = name;
            _key = key;
        }

        private string Name { get; }
        public VaultHeader Header { get; set; }
        public BlockingCollection<ItemHeader> ItemHeaders { get; } = new BlockingCollection<ItemHeader>();
        public string VaultPath { get; set; }
        public string EncryptedFolderPath => Path.Combine(VaultPath, EncryptedFolderName);
        public string UnlockedFolderPath => Path.Combine(VaultPath, UnlockedFolderName);
        public string VaultFilePath => GetVaultFilePath(VaultPath, Name);

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

        public static Vault Open(string folderPath, byte[] key)
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
            var itemHeader = ItemHeader.Create(name, path);
            var destinationPath = Path.Combine(EncryptedFolderPath, itemHeader.TargetPath);

            var hash = await UserDataFile.WriteUserDataFile(sourcePath, destinationPath,
                Header.MasterPassword.Password, itemHeader.TargetCipherIV);

            itemHeader.TargetAuthentication = hash;

            ItemHeaders.Add(itemHeader);
        }

        public async Task RemoveFile(ItemHeader header)
        {
            File.Delete(Path.Combine(EncryptedFolderPath, header.TargetPath));
            if (header.IsUnlocked) await EliminateExtracted(header);
            ItemHeaders.TryTake(out header);
        }
        
        public async Task MoveFile(ItemHeader header, string destination)
        {
            var wasUnlocked = header.IsUnlocked;
            
            if (wasUnlocked) await EliminateExtracted(header);

            header.Move(destination);

            if (wasUnlocked) await ExtractFile(header);
        }

        public async Task<bool> ExtractFile(ItemHeader header)
        {
            var encryptedSourcePath = Path.Combine(EncryptedFolderPath, header.TargetPath);
            var unlockedTarget = Path.Combine(UnlockedFolderPath, header.SecuredPlainName.PlainName);

            var hash = await UserDataFile.ExtractUserDataFile(encryptedSourcePath, unlockedTarget,
                Header.MasterPassword.Password, header.TargetCipherIV);

            header.IsUnlocked = true;

            return hash.ContentEqualTo(header.TargetAuthentication);
        }

        public async Task EliminateExtracted(ItemHeader header)
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
            throw new NotImplementedException();
        }
        
        public void CorrectUnlockedFile(ItemHeader header)
        {
            throw new NotImplementedException();
        }
        
        public bool CheckUnlockedFile(ItemHeader header)
        {
            throw new NotImplementedException();
        }

        public static bool Exists(string vaultPath)
        {
            // check if folder(go to config and check for same name) or file, check for magic number in file
            throw new NotImplementedException();
        }
        
        public void Dispose()
        {
            if (!Written)
                VaultReaderWriter.WriteConfig(this, _key);
        }
    }
}