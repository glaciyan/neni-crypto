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
    public class VaultItemHeadersFile : IDisposable
    {
        private const string FileExtension = ".vlt";
        
        private readonly byte[] _key;
        
        private string Name { get; }
        private VaultHeader Header { get; set; }
        public List<ItemHeader> ItemHeaders { get; } = new List<ItemHeader>();
        public string VaultPath { get; private set; }
        public string VaultFilePath => GetVaultFilePath(VaultPath, Name);

        private VaultItemHeadersFile(string name, byte[] key)
        {
            _key = key;
            Name = name;
        }

        public static VaultItemHeadersFile Create(string name, byte[] key) => Create(name, null, key);
        
        public static VaultItemHeadersFile Create(string name, string path, byte[] key)
        {
            var output = new VaultItemHeadersFile(name, key)
            {
                Header = VaultHeader.Create()
            };
            
            output.VaultPath = path == null ?
                Path.Combine(Environment.CurrentDirectory, name) :
                Path.GetFullPath(path + "/" + name);

            PrepareVault(output);

            return output;
        }

        private static void PrepareVault(VaultItemHeadersFile vaultFile)
        {
            Directory.CreateDirectory(vaultFile.VaultPath);
            File.Create(vaultFile.VaultFilePath).Dispose();
        }

        public async Task AddFileAsync(string sourcePath, string path = "")
        {
            var name = Path.GetFileName(sourcePath);
            var header = ItemHeader.Create(name, path);

            // TODO: encrypt file
            await using var sourceFileStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
            await using var targetFileStream = new FileStream(GetTargetPath(header), FileMode.Create, FileAccess.Write);
            var hash = await sourceFileStream.CopyToCreateHashAsync(targetFileStream);

            header.TargetAuthentication = hash;

            ItemHeaders.Add(header);
        }

        private string GetTargetPath(ItemHeader header)
        {
            return VaultPath + "/" + header.TargetPath;
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

        public static VaultItemHeadersFile ReadFrom(string folderPath, byte[] key)
        {
            Debug.Assert(folderPath != null, nameof(folderPath) + " != null");
            
            var (fullFolderPath, folderName, vaultFilePath) = VerifyPathAndGetName(folderPath);
            var result = new VaultItemHeadersFile(folderName, key);
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