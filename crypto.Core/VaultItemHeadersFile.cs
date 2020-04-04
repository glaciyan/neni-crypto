using System;
using System.Collections.Generic;
using System.IO;
using crypto.Core.File;

namespace crypto.Core
{
    public class VaultItemHeadersFile : IDisposable
    {
        public VaultHeader Header { get; set; }
        public List<ItemHeader> ItemHeaders { get; } = new List<ItemHeader>();
        private byte[] _key;

        private VaultItemHeadersFile(byte[] key)
        {
            _key = key;
        }

        public static VaultItemHeadersFile Create(byte[] key)
        {
            return new VaultItemHeadersFile(key) {Header = VaultHeader.Create()};
        }

        public void AddFile(string path, string directory = "")
        {
            ItemHeaders.Add(ItemHeader.Create(path, directory));
        }

        public void WriteToFile(string target)
        {
            using var fileStream = new FileStream(target, FileMode.Create);
            
            WriteHeader(fileStream);
            
            using var binWriter = new BinaryWriter(fileStream);
            
            foreach (var itemHeader in ItemHeaders)
            {
                WriteItemHeader(fileStream, itemHeader, binWriter);
            }
        }
        
        public static VaultItemHeadersFile ReadFrom(string path, byte[] key)
        {
            throw new NotImplementedException();
        }

        private void WriteItemHeader(Stream fileStream, ItemHeader itemHeader, BinaryWriter binWriter)
        {
            // save position start position
            var startPos = fileStream.Position;

            // make space for writing the end position
            fileStream.Position += sizeof(long);

            // write the header and then save the position
            var itemHeaderWriter = new ItemHeaderWriter(itemHeader);
            itemHeaderWriter.WriteTo(fileStream, _key);
            var endPos = fileStream.Position;

            fileStream.Position = startPos;
            binWriter.Write(endPos);
            fileStream.Position = endPos;
        }

        private void WriteHeader(Stream fileStream)
        {
            var headerWriter = new VaultHeaderWriter(Header);
            headerWriter.WriteTo(fileStream, _key);
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