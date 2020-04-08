using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using crypto.Core.ExtensionUtilities;
using crypto.Core.Header;

namespace crypto.Core.Vault
{
    public class Vault
    {
        private const string FileExtension = ".vlt";
        
        private readonly byte[] _key;
        
        private string Name { get; }
        public VaultHeader Header { get; set; }
        public List<ItemHeader> ItemHeaders { get; } = new List<ItemHeader>();
        public string VaultPath { get; set; }
        public string EncryptedFolderPath => Path.Combine(VaultPath, "Encrypted");
        public string UnlockedFolderPath => Path.Combine(VaultPath, "Unlocked");
        public string VaultFilePath => GetVaultFilePath(VaultPath, Name);
        
        internal static string GetVaultFilePath(string vaultPath, string name)
        {
            return vaultPath + "/" + name + FileExtension;
        }

        internal Vault(string name, byte[] key)
        {
            _key = key;
            Name = name;
        }

        public static Vault Create(string name, byte[] key) => Create(name, null, key);
        
        public static Vault Create(string name, string path, byte[] key)
        {
            var output = new Vault(name, key)
            {
                Header = VaultHeader.Create()
            };
            
            output.VaultPath = path == null ?
                Path.Combine(Environment.CurrentDirectory, name) :
                Path.GetFullPath(path + "/" + name);

            PrepareVault(output);

            return output;
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

        public async Task<bool> ExtractFile(ItemHeader header)
        {
            var encryptedSourcePath = Path.Combine(EncryptedFolderPath, header.TargetPath);
            var unlockedTarget = Path.Combine(UnlockedFolderPath, header.SecuredPlainName.PlainName);
            
            var hash = await UserDataFile.ExtractUserDataFile(encryptedSourcePath, unlockedTarget,
                Header.MasterPassword.Password, header.TargetCipherIV);

            return hash.ContentEqualTo(header.TargetAuthentication);
        }
    }
}