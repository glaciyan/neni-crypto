using System.IO;
using System.Security.Cryptography;

namespace crypto.Core
{
    public static class SimpleCryptography
    {
        public static byte[] EncryptBytes(byte[] plainText, byte[] key, byte[] iv)
        {
            byte[] cipherText;

            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Key = key;
            aes.IV = iv;

            using (var memoryDestinationStream = new MemoryStream())
            {
                using (var cryptoStream =
                    new CryptoStream(memoryDestinationStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainText);
                }

                cipherText = memoryDestinationStream.ToArray();
            }

            return cipherText;
        }

        public static byte[] DecryptBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            byte[] plainText;

            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Key = key;
            aes.IV = iv;

            using (var memoryDecryptedStream = new MemoryStream())
            {
                using (var cryptoStream =
                    new CryptoStream(memoryDecryptedStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(cipherText, 0, cipherText.Length);
                }

                plainText = memoryDecryptedStream.ToArray();
            }

            return plainText;
        }
    }
}