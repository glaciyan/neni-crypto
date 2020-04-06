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
                GenerateIV();
            else
                IV = iv;
        }

        public SecretFileName(byte[] encryptedName, byte[] iv, byte[] key)
        {
            IV = iv;
            SetName(encryptedName, key);
        }

        public byte[] IV { get; private set; }
        public string PlainName { get; set; }

        private static Encoding Encoder { get; } = Encoding.Unicode;

        public byte[] GetEncryptedName(byte[] key)
        {
            var plainTextPathBytes = Encoder.GetBytes(PlainName);
            using var aesEncrypt = new AesBytes(key, IV);

            return aesEncrypt.EncryptBytes(plainTextPathBytes);
        }

        private void SetName(byte[] encryptedName, byte[] key)
        {
            using var aesDecrypt = new AesBytes(key, IV);
            var name = Encoder.GetString(aesDecrypt.DecryptBytes(encryptedName));
            PlainName = name;
        }

        // TODO: move file

        private void GenerateIV()
        {
            IV = CryptoRNG.GetRandomBytes(AesSizes.IV);
        }
    }
}