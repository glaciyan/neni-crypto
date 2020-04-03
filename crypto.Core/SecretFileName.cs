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

        public SecretFileName(byte[] decryptedName, byte[] iv)
        {
            IV = iv;
            DecryptedName = decryptedName;
        }
        
        public byte[] IV { get; private set; }
        private string PlainName { get; set; }
        private byte[] DecryptedName { get; }
        
        private static Encoding Encoder { get; } = Encoding.Unicode;

        public byte[] GetEncryptedName(byte[] key)
        {
            var plainTextPathBytes = Encoder.GetBytes(PlainName);
            using var aesEncrypt = new AesBytes(key, IV);

            return aesEncrypt.EncryptBytes(plainTextPathBytes);
        }

        public string GetName(byte[] key)
        {
            using var aesDecrypt = new AesBytes(key, IV);
            return Encoder.GetString(aesDecrypt.DecryptBytes(DecryptedName));
        }
        
        private void GenerateIV()
        {
            IV = CryptoRNG.GetRandomBytes(AesSizes.IV);
        }
    }
}
