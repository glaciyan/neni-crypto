using System.IO;
using System.Security.Cryptography;

namespace crypto.Core
{
    public static class QuickCryptoStream
    {
        public static Stream GetDecryptor(Stream source, KeyIVPair keyRing)
        {
            return new CryptoStream(source,
                Aes.Create().CreateDecryptor(keyRing.Key, keyRing.IV),
                CryptoStreamMode.Read);
        }

        public static Stream GetEncryptor(Stream source, KeyIVPair keyRing)
        {
            return new CryptoStream(source,
                Aes.Create().CreateEncryptor(keyRing.Key, keyRing.IV),
                CryptoStreamMode.Read);
        }
    }
}