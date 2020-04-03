using System.IO;
using System.Text;
using crypto.Core.Cryptography;

namespace crypto.Core.File
{
    public class VaultFile
    {
        private const int CipherTextNameLength = 16;

        private string _decryptedPath;

        private VaultFile(string filePath, string plainTextParentDirPath = "")
        {
            GenerateConfigIV();
            GenerateCipherFileIV();
            IsDecrypted = false;
            PlainTextFileInfo = new FileInfo(plainTextParentDirPath + Path.GetFileName(filePath));
            CipherTextName = RandomGenerator.RandomFileName(CipherTextNameLength);
            Authentication = new byte[32];
        }

        internal VaultFile()
        {
        }

        private static Encoding Encoder { get; } = Encoding.Unicode;

        public byte[] CipherFileIV { get; set; }
        public byte[] PlainTextNameIV { get; set; }

        public bool IsDecrypted { get; set; }

        // config secret
        public FileInfo PlainTextFileInfo { get; private set; }

        public string CipherTextName { get; set; }
        private string ParentFolderPath { get; } = "";
        public byte[] CipherTextPathBytes => Encoder.GetBytes(ParentFolderPath + CipherTextName);

        public string DecryptedPath
        {
            get => _decryptedPath ?? string.Empty;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    IsDecrypted = true;
                    _decryptedPath = value;
                }
                else
                {
                    IsDecrypted = false;
                }
            }
        }

        public byte[] Authentication { get; set; }

        public static VaultFile Create(string filePath, string pathToPlain = "")
        {
            return new VaultFile(filePath, pathToPlain);
        }

        public byte[] GetEncryptedPlainTextPath(byte[] key)
        {
            var plainTextPathBytes = Encoder.GetBytes(PlainTextFileInfo.ToString());
            using var aesEncrypt = new AesBytes(key, PlainTextNameIV);

            return aesEncrypt.EncryptBytes(plainTextPathBytes);
        }

        public void SetPlainTextPathFromDecryptedBytes(byte[] decryptedPlainTextPath, byte[] key)
        {
            using var aesDecrypt = new AesBytes(key, PlainTextNameIV);
            PlainTextFileInfo = new FileInfo(Encoder.GetString(aesDecrypt.DecryptBytes(decryptedPlainTextPath)));
        }

        public void GenerateConfigIV()
        {
            PlainTextNameIV = CryptoRNG.GetRandomBytes(CryptoRNG.Aes256IVSizeInBytes);
        }

        public void GenerateCipherFileIV()
        {
            CipherFileIV = CryptoRNG.GetRandomBytes(CryptoRNG.Aes256IVSizeInBytes);
        }
    }
}