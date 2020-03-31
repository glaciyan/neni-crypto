using System;
using System.IO;

namespace crypto.Core.File.Format.CryptoConfig
{
    public class VaultFileFormat
    {
        private VaultFile _vaultFile;

        public VaultFileFormat(VaultFile vaultFile)
        {
            _vaultFile = vaultFile;
        }

        public VaultFileFormat()
        {
        }

        public byte[] GetHeader(byte[] key)
        {
            if (_vaultFile == null) throw new NullReferenceException();
            
            var cipherFileIV = _vaultFile.CipherFileIV;
            var plainTextNameIV = _vaultFile.PlainTextNameIV;
            var isDecrypted = BitConverter.GetBytes(_vaultFile.IsDecrypted);
            var plainTextPath = new RandomLengthFileContent(_vaultFile.GetEncryptedPlainTextPath(key));
            var cipherTextPath = _vaultFile.CipherTextPathBytes;
            var authentication = _vaultFile.Authentication;

            var format = new[]
            {
                cipherFileIV,
                plainTextNameIV,
                isDecrypted,
                plainTextPath.GetBytes(),
                cipherTextPath,
                authentication
            };

            var sum = 0;
            foreach (var b in format)
            {
                sum += b.Length;
            }

            var output = new byte[sum];
            output.CombineFrom(format);

            return output;
        }

        public void FromStream(Stream source, byte[] key)
        {
            throw new NotImplementedException();
        }
    }
}