using System.IO;
using crypto.Core.Cryptography;

namespace crypto.Core.File
{
    public class HeaderFile
    {
        private const int CipherTextNameLength = 16;

        private HeaderFile(string filePath, string plainTextParentDirPath = "")
        {
            SecuredPlainName = new SecretFileName(plainTextParentDirPath + Path.GetFileName(filePath));
            GenerateCipherFileIV();
            TargetPath = RandomGenerator.RandomFileName(CipherTextNameLength);
            TargetAuthentication = new byte[32];
        }

        public HeaderFile()
        {
        }

        public static HeaderFile Create(string plainFileName, string pathToPlain = "")
        {
            return new HeaderFile(plainFileName, pathToPlain);
        }

        public static HeaderFile ReadFrom(Stream source)
        {
            return HeaderFileReader.ReadFrom(source);
        }

        public SecretFileName SecuredPlainName { get; set; }
        
        public byte[] TargetCipherIV { get; set; }
        public byte[] TargetAuthentication { get; set; } = new byte[AesSizes.Auth];
        public string TargetPath { get; set; }

        public bool IsUnlocked => UnlockedFilePath != null;
        public SecretFileName UnlockedFilePath { get; set; }

        public void GenerateCipherFileIV()
        {
            TargetCipherIV = CryptoRNG.GetRandomBytes(AesSizes.IV);
        }
    }
}