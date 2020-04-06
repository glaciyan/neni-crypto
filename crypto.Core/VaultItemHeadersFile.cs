using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
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

        private VaultItemHeadersFile(string name, byte[] key)
        {
            _key = key;
            Name = name;
        }

        public static VaultItemHeadersFile Create(string name, byte[] key)
        {
            return new VaultItemHeadersFile(name, key) {Header = VaultHeader.Create()};
        }

        public void AddFile(string path, string directory = "")
        {
            var header = ItemHeader.Create(path, directory);
            
            // write the UserData and set the auth key
            throw new NotImplementedException();
            ItemHeaders.Add(header);
        }
        
        public bool ExtractFile(ItemHeader header, string folderPath)
        {
            throw new NotImplementedException();
        }

        public void WriteTo(string folderPath)
        {
            using var fileStream = new FileStream(folderPath + Name + FileExtension, FileMode.Create);
            
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

        public static VaultItemHeadersFile ReadFrom(string source, byte[] key)
        {
            Debug.Assert(source != null, nameof(source) + " != null");
            
            var result = new VaultItemHeadersFile(Path.GetFileNameWithoutExtension(source), key);
            using var fileStream = new FileStream(source, FileMode.Open, FileAccess.Read);
            
            result.Header = VaultHeaderReader.ReadFrom(fileStream);

            var (keyWasCorrect, password) = result.Header.MasterPassword.GetDecryptedPassword(key);
            
            if (!keyWasCorrect) throw new CryptographicException("Wrong Password");
            
            while(fileStream.Position < fileStream.Length)
            {
                result.ItemHeaders.Add(ItemHeaderReader.ReadFrom(fileStream, password));
            }

            return result;
        }
        
        public void Dispose()
        {
            for (var i = 0; i < _key.Length; i++)
            {
                _key[i] = 0;
            }
        }
    }
}