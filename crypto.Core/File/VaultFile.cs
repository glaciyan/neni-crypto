using System.IO;
using System.Text;
using crypto.Core.Cryptography;

namespace crypto.Core.File
{
    public class VaultFile
    {
        private const int CipherTextNameLength = 16;
        private static Encoding Encoder { get; } = Encoding.Unicode;

        public static VaultFile Create(string filePath, string pathToPlain)
        {
            return new VaultFile(filePath, pathToPlain);
        }

        private VaultFile(string filePath, string plainTextParentDirPath)
        {
            GenerateConfigIV();
            GenerateCipherFileIV();
            IsDecrypted = false;
            PlainTextFileInfo = new FileInfo(plainTextParentDirPath + Path.GetFileName(filePath));
            CipherTextName = RandomGenerator.RandomFileName(CipherTextNameLength);
        }

        public byte[] CipherFileIV { get; set; }
        public byte[] PlainTextNameIV { get; set; }

        public bool IsDecrypted { get; set; }
        
        // config secret
        public FileInfo PlainTextFileInfo { get; private set; }

        private string CipherTextName { get; set; }

        public byte[] CipherTextPathBytes => Encoder.GetBytes(CipherTextName);
        
        
        public string DecryptedPath { get; set; }

        public byte[] Authentication { get; set; }

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