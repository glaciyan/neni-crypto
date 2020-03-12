using System.IO;
using System.Security.Cryptography;

namespace crypto.Core
{
    public static class CryptoFileIO
    {
        public static void Write(CryptoFile cryptoFile, KeyIVPair keyRing, string destination)
        {
            using var sourceStream = cryptoFile.FileInfo.OpenRead();
            using var cryptoStream = new CryptoStream(sourceStream,
                Aes.Create().CreateEncryptor(keyRing.Key, keyRing.IV),
                CryptoStreamMode.Read);

            using var destinationStream = new FileStream(destination, FileMode.Create);

            cryptoStream.CopyTo(destinationStream);
        }
    }
}