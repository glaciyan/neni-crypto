using System;
using System.Diagnostics;
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

        private ItemHeader(string fileName, string plainTextParentDirPath = "")
        {
            Debug.Assert(fileName != null, nameof(fileName) + " != null");
            var plainPath = Path.Combine(plainTextParentDirPath.Replace('\\', '/'), Path.GetFileName(fileName));
            SecuredPlainName = new SecretFileName(plainPath);
            GenerateCipherFileIV();
            TargetPath = RandomGenerator.RandomFileName(CipherTextNameLength);
            TargetAuthentication = new byte[32];
        }

        public SecretFileName SecuredPlainName { get; set; }

        public byte[] TargetCipherIV { get; set; }
        public byte[] TargetAuthentication { get; set; } = new byte[AesSizes.Auth];
        public string TargetPath { get; set; }

        public bool IsUnlocked { get; set; }

        public static ItemHeader Create(string plainFileName, string pathToPlain = "")
        {
            return new ItemHeader(plainFileName, pathToPlain);
        }

        public void Move(string destination)
        {
            SecuredPlainName.PlainName = NPath.RemoveRelativeParts(destination);
        }

        private void GenerateCipherFileIV()
        {
            TargetCipherIV = CryptoRNG.GetRandomBytes(AesSizes.IV);
        }
    }
}