// using System;
// using System.IO;
//
// namespace crypto.Core.Deprecated
// {
//     public class VaultItemInfoIO
//     {
//         private readonly VaultItemInfoParser _parser;
//
//         public VaultItemInfoIO(VaultItemInfoParser parser)
//         {
//             _parser = parser;
//         }
//
//         public VaultItemInfoIO()
//         {
//         }
//
//         public void Write(string folder, byte[] key)
//         {
//             if (_parser == null) throw new NullReferenceException();
//
//             using var destination = new FileStream(folder + _parser.ItemInfo.CipherTextFileName, FileMode.OpenOrCreate);
//
//             destination.Write(new[] {_parser.GetByteFlags()});
//             destination.Write(_parser.GetCipherIv());
//             destination.Write(_parser.GetPlainNameCipherIv());
//             _parser.GetPlainFileName(key).WriteTo(destination);
//         }
//
//         public void Read(byte[] key)
//         {
//             var flags = new byte[1];
//             var cipherIv = new byte[16];
//             var plainNameCipherIv = new byte[16];
//             var plainName = new RandomLengthFileContent();
//
//             using var source = new FileStream(_parser.ItemInfo.CipherTextFile.Path, FileMode.Open);
//
//             source.Read(flags);
//             source.Read(cipherIv);
//             source.Read(plainNameCipherIv);
//             plainName.ReadFrom(source);
//
//             var plainNameDecryptedString = VaultItemInfoParser.GetPlainFileName(plainName.Content, key, plainNameCipherIv);
//
//             _parser.ItemInfo.IsDecryptedInVault = flags[0] == 1;
//             _parser.ItemInfo.CipherIv = cipherIv;
//             _parser.ItemInfo.PlainTextNameCipherIV = plainNameCipherIv;
//             _parser.ItemInfo.PlainTextName = plainNameDecryptedString;
//         }
//     }
// }

