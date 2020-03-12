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

        public static CryptoFile WriteDecrypted(string source, KeyIVPair keyRing, string destination)
        {
            var sourceFileInfo = new FileInfo(source);

            using var sourceStream = sourceFileInfo.OpenRead();
            using var cryptoStream = new CryptoStream(sourceStream,
                Aes.Create().CreateDecryptor(keyRing.Key, keyRing.IV),
                CryptoStreamMode.Read);

            var destinationFileInfo = new FileInfo(destination);

            using var destinationStream = destinationFileInfo.OpenWrite();

            cryptoStream.CopyTo(destinationStream);

            return new CryptoFile(destination, destinationFileInfo);
        }
    }
}