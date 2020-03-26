using System.IO;

namespace crypto.Core
{
    public class VaultItemInfo
    {
        public CipherTextFile CipherTextFile { get; }
        public PlainTextFile PlainTextFile { get; }

        public string PlainTextName { get; set; }
        public string CipherTextFileName => CipherTextFile.FileInfo.Name;

        private byte[] _plainTextNameCipherIv;
        public byte[] PlainTextNameCipherIV
        {
            get
            {
                _plainTextNameCipherIv ??= CryptoRNG.GetRandomBytes(CryptoRNG.Aes256IvSizeInBytes);
                return _plainTextNameCipherIv;
            }
            set => _plainTextNameCipherIv = value;
        }

        public bool IsDecryptedInVault { get; set; }

        private byte[] _cipherIv;
        public byte[] CipherIv
        {
            get
            {
                _cipherIv ??= CryptoRNG.GetRandomBytes(CryptoRNG.Aes256IvSizeInBytes);
                return _cipherIv;
            }
            set => _cipherIv = value;
        }

        public VaultItemInfo(PlainTextFile plainText)
        {
            CipherTextFile = new CipherTextFile(RandomGenerator.RandomFileName(CipherTextFile.NameLength));
            PlainTextName = plainText.Name;
            IsDecryptedInVault = false;
        }

        public VaultItemInfo(CipherTextFile cipherText)
        {
            CipherTextFile = cipherText;
        }
    }
}
