using System.IO;

namespace crypto.Core
{
    public static class CryptoFileIO
    {
        public static void Write(CipherFile cipherFile, KeyIVPair keyRing, string destination)
        {
            using var sourceStream = cipherFile.FileInfo.OpenRead();
            using var cryptoStream = QuickCryptoStream.GetEncryptor(sourceStream, keyRing);

            using var destinationStream = new FileStream(destination, FileMode.Create);

            cryptoStream.CopyTo(destinationStream);
        }

        public static CipherFile WriteDecrypted(string source, KeyIVPair keyRing, string destination)
        {
            var sourceFileInfo = new FileInfo(source);

            using var sourceStream = sourceFileInfo.OpenRead();
            using var cryptoStream = QuickCryptoStream.GetDecryptor(sourceStream, keyRing);

            var destinationFileInfo = new FileInfo(destination);

            using var destinationStream = destinationFileInfo.OpenWrite();

            cryptoStream.CopyTo(destinationStream);

            return new CipherFile(destination, destinationFileInfo);
        }
    }
}