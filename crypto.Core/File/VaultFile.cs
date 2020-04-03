using System.IO;
using crypto.Core.Cryptography;

namespace crypto.Core.File
{
    public class VaultFile
    {
        private const int CipherTextNameLength = 16;

        private VaultFile(string filePath, string plainTextParentDirPath = "")
        {
            SecuredPlainName = new SecretFileName(plainTextParentDirPath + Path.GetFileName(filePath));
            GenerateCipherFileIV();
            TargetPath = RandomGenerator.RandomFileName(CipherTextNameLength);
            TargetAuthentication = new byte[32];
        }

        public VaultFile()
        {
        }

        public static VaultFile Create(string plainFileName, string pathToPlain = "")
        {
            return new VaultFile(plainFileName, pathToPlain);
        }

        public SecretFileName SecuredPlainName { get; set; }
        
        public byte[] TargetCipherIV { get; set; }
        public byte[] TargetAuthentication { get; set; }
        public string TargetPath { get; set; }

        public bool IsUnlocked => UnlockedFilePath != null;
        public SecretFileName UnlockedFilePath { get; set; }

        public void GenerateCipherFileIV()
        {
            TargetCipherIV = CryptoRNG.GetRandomBytes(AesSizes.IV);
        }
    }
}