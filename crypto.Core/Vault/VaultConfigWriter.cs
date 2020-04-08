using System;
using System.IO;
using crypto.Core.ExtensionUtilities;
using crypto.Core.Header;

namespace crypto.Core.Vault
{
    public class VaultConfigWriter : IDisposable
    {
        private readonly Vault _underlying;
        private readonly byte[] _key;

        public VaultConfigWriter(Vault underlying, byte[] key)
        {
            _underlying = underlying;
            _key = key;
        }

        public void Write()
        {
            using var fileStream = new FileStream(_underlying.VaultFilePath, FileMode.Open);
            
            WriteHeader(fileStream);
            
            using var binWriter = new BinaryWriter(fileStream);
            
            foreach (var itemHeader in _underlying.ItemHeaders)
            {
                WriteItemHeader(fileStream, itemHeader);
            }
        }

        private void WriteHeader(Stream fileStream)
        {
            var headerWriter = new VaultHeaderWriter(_underlying.Header);
            headerWriter.WriteTo(fileStream, _key);
        }

        private void WriteItemHeader(Stream fileStream, ItemHeader itemHeader)
        {
            var itemHeaderWriter = new ItemHeaderWriter(itemHeader);
            itemHeaderWriter.WriteTo(fileStream, _underlying.Header.MasterPassword.Password);
        }

        public void Dispose()
        {
            _key.Zeros();
        }
    }
}