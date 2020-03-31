using System.IO;
using System.Security.Cryptography;
using crypto.Core.Cryptography;

namespace crypto.Core
{
    public static class QuickCryptoStream
    {
        public static Stream GetDecryptor(Stream source, KeyIVPair keyRing)
        {
            using var aes = GetAes256(keyRing.Key, keyRing.IV);

            var cryptoStream = new CryptoStream(source,
                aes.CreateDecryptor(),
                CryptoStreamMode.Read);

            return cryptoStream;
        }

        public static Stream GetEncryptor(Stream source, KeyIVPair keyRing)
        {
            using var aes = GetAes256(keyRing.Key, keyRing.IV);

            return new CryptoStream(source,
                aes.CreateEncryptor(),
                CryptoStreamMode.Read);
        }

        private static Aes GetAes256(byte[] key, byte[] iv)
        {
            var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Key = key;
            aes.IV = iv;

            return aes;
        }
    }
}