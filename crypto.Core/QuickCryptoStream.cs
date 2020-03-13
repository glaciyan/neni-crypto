using System.IO;
using System.Security.Cryptography;

namespace crypto.Core
{
    public static class QuickCryptoStream
    {
        public static Stream GetStream(Stream source, KeyIVPair keyRing)
        {
            return new CryptoStream(source,
                Aes.Create().CreateDecryptor(keyRing.Key, keyRing.IV),
                CryptoStreamMode.Read);
        }
    }
}