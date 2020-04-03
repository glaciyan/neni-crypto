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

        public static VaultFile Create(string filePath, string pathToPlain = "")
        {
            return new VaultFile(filePath, pathToPlain);
        }

        public static VaultFile Open(string filePath)
        {
            return new VaultFile(filePath);
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
            TargetCipherIV = CryptoRNG.GetRandomBytes(CryptoRNG.Aes256IVSizeInBytes);
        }
    }
}