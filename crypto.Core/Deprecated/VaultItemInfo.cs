// using crypto.Core.Cryptography;
//
// namespace crypto.Core.Deprecated
// {
//     public class VaultItemInfo
//     {
//         private byte[] _cipherIv;
//
//         private byte[] _plainTextNameCipherIv;
//
//         public VaultItemInfo(PlainTextFile plainText)
//         {
//             CipherTextFile = new CipherTextFile(RandomGenerator.RandomFileName(CipherTextFile.NameLength));
//             PlainTextName = plainText.Name;
//             IsDecryptedInVault = false;
//         }
//
//         public VaultItemInfo(CipherTextFile cipherText)
//         {
//             CipherTextFile = cipherText;
//         }
//
//         public CipherTextFile CipherTextFile { get; }
//         public PlainTextFile PlainTextFile { get; }
//
//         public string PlainTextName { get; set; }
//         public string CipherTextFileName => CipherTextFile.FileInfo.Name;
//
//         public byte[] PlainTextNameCipherIV
//         {
//             get
//             {
//                 _plainTextNameCipherIv ??= CryptoRNG.GetRandomBytes(CryptoRNG.Aes256IVSizeInBytes);
//                 return _plainTextNameCipherIv;
//             }
//             set => _plainTextNameCipherIv = value;
//         }
//
//         public bool IsDecryptedInVault { get; set; }
//
//         public byte[] CipherIv
//         {
//             get
//             {
//                 _cipherIv ??= CryptoRNG.GetRandomBytes(CryptoRNG.Aes256IVSizeInBytes);
//                 return _cipherIv;
//             }
//             set => _cipherIv = value;
//         }
//     }
// }