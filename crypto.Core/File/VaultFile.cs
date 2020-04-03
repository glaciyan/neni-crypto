using System.IO;
using crypto.Core.Cryptography;

namespace crypto.Core.File
{
    public class VaultFile
    {
        private const int CipherTextNameLength = 16;

        private string _unlockedFilePath;

        private VaultFile(string filePath, string plainTextParentDirPath = "")
        {
            SecuredPlainName = new SecretFileName(plainTextParentDirPath + Path.GetFileName(filePath));
            GenerateCipherFileIV();
            IsUnlocked = false;
            TargetPath = RandomGenerator.RandomFileName(CipherTextNameLength);
            TargetAuthentication = new byte[32];
        }

        public static VaultFile Create(string plainFileName, string pathToPlain = "")
        {
            return new VaultFile(plainFileName, pathToPlain);
        }

        public static VaultFile Open(string plainFileName)
        {
            return new VaultFile(plainFileName);
        }

        public SecretFileName SecuredPlainName { get; set; }
        
        public byte[] TargetCipherIV { get; set; }
        public byte[] TargetAuthentication { get; set; }
        public string TargetPath { get; set; }

        public bool IsUnlocked { get; private set; }
        public string UnlockedFilePath
        {
            get => _unlockedFilePath ?? string.Empty;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    IsUnlocked = true;
                    _unlockedFilePath = value;
                }
                else
                {
                    IsUnlocked = false;
                }
            }
        }

        public void GenerateCipherFileIV()
        {
            TargetCipherIV = CryptoRNG.GetRandomBytes(AesSizes.IV);
        }
    }
}