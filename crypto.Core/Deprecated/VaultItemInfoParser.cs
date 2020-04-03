// using System.Text;
// using crypto.Core.Cryptography;
//
// namespace crypto.Core.Deprecated
// {
//     public class VaultItemInfoParser
//     {
//         public static readonly Encoding NameEncoding = Encoding.Unicode;
//
//         public VaultItemInfoParser(VaultItemInfo itemInfo)
//         {
//             ItemInfo = itemInfo;
//         }
//
//         public VaultItemInfo ItemInfo { get; set; }
//
//         public byte GetByteFlags()
//         {
//             return ItemInfo.IsDecryptedInVault ? (byte) 1 : (byte) 0;
//         }
//
//         public byte[] GetCipherIv()
//         {
//             return ItemInfo.CipherIv;
//         }
//
//         public byte[] GetPlainNameCipherIv()
//         {
//             return ItemInfo.PlainTextNameCipherIV;
//         }
//
//         public RandomLengthFileContent GetPlainFileName(byte[] key)
//         {
//             var encodedName = NameEncoding.GetBytes(ItemInfo.PlainTextName);
//             using var aes = new AesBytes(key, ItemInfo.PlainTextNameCipherIV);
//
//             return new RandomLengthFileContent(aes.EncryptBytes(encodedName));
//         }
//
//         public static string GetPlainFileName(byte[] data, byte[] key, byte[] iv)
//         {
//             using var aes = new AesBytes(key, iv);
//
//             return NameEncoding.GetString(aes.DecryptBytes(data));
//         }
//     }
// }

