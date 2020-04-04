using System.IO;
using System.Text;
using crypto.Core.Cryptography;

namespace crypto.Core
{
    public class SecretFileName
    {
        public SecretFileName(string plainName, byte[] iv = null)
        {
            PlainName = plainName;

            if (iv == null)
            {
                GenerateIV();
            }
            else
            {
                this.IV = iv;
            }
        }

        public SecretFileName(byte[] encryptedName, byte[] iv)
        {
            IV = iv;
            EncryptedName = encryptedName;
        }
        
        public byte[] IV { get; private set; }
        private string PlainName { get; set; }
        private byte[] EncryptedName { get; }
        
        private static Encoding Encoder { get; } = Encoding.Unicode;

        public byte[] GetEncryptedName(byte[] key)
        {
            var plainTextPathBytes = Encoder.GetBytes(PlainName);
            using var aesEncrypt = new AesBytes(key, IV);

            return aesEncrypt.EncryptBytes(plainTextPathBytes);
        }

        public string GetName(byte[] key = null)
        {
            if (EncryptedName == null || key == null)
            {
                return PlainName;
            }
            
            using var aesDecrypt = new AesBytes(key, IV);
            return Encoder.GetString(aesDecrypt.DecryptBytes(EncryptedName));
        }
        
        // TODO: move file
        
        private void GenerateIV()
        {
            IV = CryptoRNG.GetRandomBytes(AesSizes.IV);
        }
    }
}
