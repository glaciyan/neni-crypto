using System.IO;
using crypto.Core.Cryptography;

namespace crypto.Core.File
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

        public SecretFileName SecuredPlainName { get; set; }

        public byte[] TargetCipherIV { get; set; }
        public byte[] TargetAuthentication { get; set; } = new byte[AesSizes.Auth];
        public string TargetPath { get; set; }

        public bool IsUnlocked => UnlockedFilePath != null;
        public SecretFileName UnlockedFilePath { get; set; }

        public string FilePath => UnlockedFilePath.PlainName;

        public static ItemHeader Create(string plainFileName, string pathToPlain = "")
        {
            return new ItemHeader(plainFileName, pathToPlain);
        }

        public void GenerateCipherFileIV()
        {
            TargetCipherIV = CryptoRNG.GetRandomBytes(AesSizes.IV);
        }
    }
}