using System;
using System.IO;
using System.Text;
using crypto.Core.Cryptography;

namespace crypto.Core.Header
{
    public class ItemHeader
    {
        private const int CipherTextNameLength = 16;

        private ItemHeader(string filePath, string plainTextParentDirPath = "")
        {
            SecuredPlainName = new SecretFileName(plainTextParentDirPath + Path.GetFileName(filePath));
            GenerateCipherFileIV();
            TargetPath = RandomGenerator.RandomFileName(CipherTextNameLength);
            TargetAuthentication = new byte[32];
        }

        public ItemHeader()
        {
        }
        
        public static ItemHeader Create(string plainFileName, string pathToPlain = "")
        {
            return new ItemHeader(plainFileName, pathToPlain);
        }

        public SecretFileName SecuredPlainName { get; set; }

        public byte[] TargetCipherIV { get; set; }
        public byte[] TargetAuthentication { get; set; } = new byte[AesSizes.Auth];
        public string TargetPath { get; set; }

        public bool IsUnlocked => UnlockedFilePath != null;
        public SecretFileName UnlockedFilePath { get; set; }

        public string FilePath => UnlockedFilePath.PlainName;

        public void Move(string destination)
        {
            SecuredPlainName.PlainName = RemoveRelativePathParts(destination);
        }

        private static string RemoveRelativePathParts(string path)
        {
            var splitPath = path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);

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