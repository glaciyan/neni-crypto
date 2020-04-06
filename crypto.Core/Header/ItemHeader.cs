using System;
using System.IO;
using System.Text;
using crypto.Core.Cryptography;

namespace crypto.Core.Header
{
    public class ItemHeader
    {
        private const int CipherTextNameLength = 16;

        public ItemHeader()
        {
        }

        public static ItemHeader Create(string plainFileName, string pathToPlain = "")
        {
            return new ItemHeader(plainFileName, pathToPlain);
        }

        private ItemHeader(string fileName, string plainTextParentDirPath = "")
        {
            SecuredPlainName = new SecretFileName(plainTextParentDirPath.Replace('\\', '/') + Path.GetFileName(fileName));
            GenerateCipherFileIV();
            TargetPath = RandomGenerator.RandomFileName(CipherTextNameLength);
            TargetAuthentication = new byte[32];
        }

        public SecretFileName SecuredPlainName { get; set; }

        public byte[] TargetCipherIV { get; set; }
        public byte[] TargetAuthentication { get; set; } = new byte[AesSizes.Auth];
        public string TargetPath { get; set; }

        public bool IsUnlocked => UnlockedFilePath != null;
        public SecretFileName UnlockedFilePath { get; set; }

        public void Move(string destination)
        {
            SecuredPlainName.PlainName = RemoveRelativePathParts(destination);
        }

        private static string RemoveRelativePathParts(string path)
        {
            var splitPath = path.Replace('\\', '/').Split('/', StringSplitOptions.RemoveEmptyEntries);

            var outputPath = new StringBuilder();

            foreach (var part in splitPath)
            {
                if (part != ".." && part != ".") outputPath.Append(part);
            }

            return outputPath.ToString();
        }

        private void GenerateCipherFileIV()
        {
            TargetCipherIV = CryptoRNG.GetRandomBytes(AesSizes.IV);
        }
    }
}