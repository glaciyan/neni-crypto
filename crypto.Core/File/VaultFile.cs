using System;
using System.IO;
using System.Text;
using crypto.Core.Cryptography;

namespace crypto.Core.File
{
    public class VaultFile
    {
        private const int CipherTextNameLength = 16;

        private string _unlockedFilePath;

        private VaultFile(string filePath, string plainTextParentDirPath = "")
        {
            GenerateConfigIV();
            GenerateCipherFileIV();
            IsDecrypted = false;
            PlainTextFileInfo = new FileInfo(plainTextParentDirPath + Path.GetFileName(filePath));
            CipherTextPath = RandomGenerator.RandomFileName(CipherTextNameLength);
            Authentication = new byte[32];
        }

        public VaultFile()
        {
        }

        public static VaultFile Create(string filePath, string pathToPlain = "")
        {
            return new VaultFile(filePath, pathToPlain);
        }

        private static Encoding Encoder { get; } = Encoding.Unicode;

        
        public byte[] DecryptionIV { get; set; }
        public byte[] Authentication { get; set; }
        public string CipherTextPath { get; set; }

        public byte[] NameCryptoIV { get; set; }


        // config secret

        public bool IsDecrypted { get; set; }

        public string UnlockedFilePath
        {
            get => _unlockedFilePath ?? string.Empty;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    IsDecrypted = true;
                    _unlockedFilePath = value;
                }
                else
                {
                    IsDecrypted = false;
                }
            }
        }

        #region PlainTextName

        private FileInfo PlainTextFileInfo { get; set; }

        public byte[] GetEncryptedPlainTextPath(byte[] key)
        {
            var plainTextPathBytes = Encoder.GetBytes(PlainTextFileInfo.ToString());
            using var aesEncrypt = new AesBytes(key, NameCryptoIV);

            return aesEncrypt.EncryptBytes(plainTextPathBytes);
        }

        public void SetPlainTextPathFromDecryptedBytes(byte[] decryptedPlainTextPath, byte[] key)
        {
            using var aesDecrypt = new AesBytes(key, NameCryptoIV);
            PlainTextFileInfo = new FileInfo(Encoder.GetString(aesDecrypt.DecryptBytes(decryptedPlainTextPath)));
        }
        
        #endregion
        
        public void GenerateConfigIV()
        {
            NameCryptoIV = CryptoRNG.GetRandomBytes(CryptoRNG.Aes256IVSizeInBytes);
        }

        public void GenerateCipherFileIV()
        {
            DecryptionIV = CryptoRNG.GetRandomBytes(CryptoRNG.Aes256IVSizeInBytes);
        }
    }
}