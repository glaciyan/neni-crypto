using System;
using System.IO;
using System.Text;
using crypto.Core.Cryptography;

namespace crypto.Core.File
{
    public class VaultFile
    {
        private const int CipherTextNameLength = 16;

        private string _decryptedPath;
        private string _cipherTextName;

        private VaultFile(string filePath, string plainTextParentDirPath = "")
        {
            GenerateConfigIV();
            GenerateCipherFileIV();
            IsDecrypted = false;
            PlainTextFileInfo = new FileInfo(plainTextParentDirPath + Path.GetFileName(filePath));
            CipherTextName = RandomGenerator.RandomFileName(CipherTextNameLength);
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

        public bool IsDecrypted { get; set; }
        public byte[] DecryptionIV { get; set; }
        public byte[] NameIV { get; set; }

        public string CipherTextName
        {
            get
            {
                if (CipherTextDirectory == null)
                {
                    return _cipherTextName;
                }
                
                if (CipherTextDirectory[^0] == Path.PathSeparator)
                {
                    return CipherTextDirectory + _cipherTextName;
                }
                
                return CipherTextDirectory + _cipherTextName;
            }
            
            set => _cipherTextName = value;
        }


        // config secret
        private FileInfo PlainTextFileInfo { get; set; }
        public byte[] Authentication { get; set; }

        public string CipherTextDirectory { get; set; }


        public string UnlockedFilePath
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

        public byte[] GetEncryptedPlainTextPath(byte[] key)
        {
            var plainTextPathBytes = Encoder.GetBytes(PlainTextFileInfo.ToString());
            using var aesEncrypt = new AesBytes(key, NameIV);

            return aesEncrypt.EncryptBytes(plainTextPathBytes);
        }

        public void SetPlainTextPathFromDecryptedBytes(byte[] decryptedPlainTextPath, byte[] key)
        {
            using var aesDecrypt = new AesBytes(key, NameIV);
            PlainTextFileInfo = new FileInfo(Encoder.GetString(aesDecrypt.DecryptBytes(decryptedPlainTextPath)));
        }

        public void GenerateConfigIV()
        {
            NameIV = CryptoRNG.GetRandomBytes(CryptoRNG.Aes256IVSizeInBytes);
        }

        public void GenerateCipherFileIV()
        {
            DecryptionIV = CryptoRNG.GetRandomBytes(CryptoRNG.Aes256IVSizeInBytes);
        }
    }
}