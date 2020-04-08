using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using crypto.Core.Cryptography;
using crypto.Core.Header;

namespace crypto.Core
{
    public class Vault : IDisposable
    {
        private const string FileExtension = ".vlt";
        
        private readonly byte[] _key;
        
        private string Name { get; }
        private VaultHeader Header { get; set; }
        public List<ItemHeader> ItemHeaders { get; } = new List<ItemHeader>();
        public string VaultPath { get; private set; }
        public string EncryptedFolderPath => Path.Combine(VaultPath, "Encrypted");
        public string UnlockedFolderPath => Path.Combine(VaultPath, "Unlocked");
        public string VaultFilePath => GetVaultFilePath(VaultPath, Name);

        private Vault(string name, byte[] key)
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
            File.Create(vaultFile.VaultFilePath).Dispose();
        }

        public async Task AddFileAsync(string sourcePath, string path = "")
        {
            Debug.Assert(sourcePath != null, nameof(sourcePath) + " != null");
            
            var name = Path.GetFileName(sourcePath);
            var itemHeader = ItemHeader.Create(name, path);
            
            var hash = await WriteUserDataFile(sourcePath, itemHeader);

            itemHeader.TargetAuthentication = hash;

            ItemHeaders.Add(itemHeader);
        }

        private async Task<byte[]> WriteUserDataFile(string sourcePath, ItemHeader itemHeader)
        {
            await using var sourceFileStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
            await using var targetFileStream = new FileStream(Path.Combine(EncryptedFolderPath, itemHeader.TargetPath), FileMode.Create, FileAccess.Write);

            using var decryptor = CreateEncryptor(itemHeader);
            await using var targetFileCryptoStream = new CryptoStream(targetFileStream, decryptor, CryptoStreamMode.Write);

            var hash = await sourceFileStream.CopyToCreateHashAsync(targetFileCryptoStream);
            return hash;
        }

        private ICryptoTransform CreateEncryptor(ItemHeader itemHeader)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Key = Header.MasterPassword.Password;
            aes.IV = itemHeader.TargetCipherIV;
            aes.Padding = PaddingMode.PKCS7;
            var decryptor = aes.CreateEncryptor();
            return decryptor;
        }

        public bool ExtractFile(ItemHeader header, string folderPath)
        {
            throw new NotImplementedException();
        }

        private void Write()
        {
            using var fileStream = new FileStream(VaultFilePath, FileMode.Open);
            
            WriteHeader(fileStream);
            
            using var binWriter = new BinaryWriter(fileStream);
            
            foreach (var itemHeader in ItemHeaders)
            {
                WriteItemHeader(fileStream, itemHeader);
            }
        }

        private void WriteHeader(Stream fileStream)
        {
            var headerWriter = new VaultHeaderWriter(Header);
            headerWriter.WriteTo(fileStream, _key);
        }

        private void WriteItemHeader(Stream fileStream, ItemHeader itemHeader)
        {
            var itemHeaderWriter = new ItemHeaderWriter(itemHeader);
            itemHeaderWriter.WriteTo(fileStream, Header.MasterPassword.Password);
        }

        public static Vault ReadFrom(string folderPath, byte[] key)
        {
            Debug.Assert(folderPath != null, nameof(folderPath) + " != null");
            
            var (fullFolderPath, folderName, vaultFilePath) = VerifyPathAndGetName(folderPath);
            var result = new Vault(folderName, key);
            result.VaultPath = fullFolderPath;

            using var vaultFile = new FileStream(vaultFilePath, FileMode.Open, FileAccess.Read);
            
            result.Header = VaultHeaderReader.ReadFrom(vaultFile);

            var (keyWasCorrect, password) = result.Header.MasterPassword.GetDecryptedPassword(key);
            
            if (!keyWasCorrect) throw new CryptographicException("Wrong Password");
            
            while(vaultFile.Position < vaultFile.Length)
            {
                result.ItemHeaders.Add(ItemHeaderReader.ReadFrom(vaultFile, password));
            }

            return result;
        }

        private static (string, string, string) VerifyPathAndGetName(string path)
        {
            var fullPath = Path.GetFullPath(path);
            var folderName = Path.GetFileNameWithoutExtension(fullPath);
            var vaultFilePath = GetVaultFilePath(fullPath, folderName);
            
            if (File.Exists(vaultFilePath))
            {
                return (fullPath, folderName, vaultFilePath);
            }

            throw new FileNotFoundException("Couldn't find vault file for path: " + path);
        }

        private static string GetVaultFilePath(string vaultPath, string name)
        {
            return vaultPath + "/" + name + FileExtension;
        }

        public void Dispose()
        {
            Write();
            
            for (var i = 0; i < _key.Length; i++)
            {
                _key[i] = 0;
            }
        }
    }
}