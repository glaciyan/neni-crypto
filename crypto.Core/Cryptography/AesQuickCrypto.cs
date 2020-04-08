using System;
using System.IO;
using System.Security.Cryptography;

namespace crypto.Core.Cryptography
{
    public class AesQuickCrypto : IDisposable
    {
        private readonly Aes _aes;

        public AesQuickCrypto(KeyIVPair keyIvPair)
        {
            var keyIvPair1 = keyIvPair;

            // create aes with preferred settings
            _aes = Aes.Create();
            _aes.KeySize = 256;
            _aes.Key = keyIvPair1.Key;
            _aes.IV = keyIvPair1.IV;
            _aes.Padding = PaddingMode.PKCS7;
        }

        public AesQuickCrypto(byte[] key, byte[] iv) : this(new KeyIVPair(key, iv))
        {
        }

        public void Dispose()
        {
            _aes?.Dispose();
        }

        public byte[] EncryptBytes(byte[] plainText)
        {
            // // get the size with spacing for padding
            // int outputSize;
            // if (plainText.Length % AesBlockSize != 0)
            // {
            //     outputSize = plainText.Length + (AesBlockSize - plainText.Length % AesBlockSize);
            // }
            // else
            // {
            //     outputSize = plainText.Length;
            // }
            // var outputBuffer = new byte[outputSize];
            //
            // 
            //
            // // counts the transformed bytes
            // var transformed = 0;
            // while (transformed < plainText.Length - AesBlockSize)
            //     transformed += encryptTransform.TransformBlock(
            //         plainText, transformed, AesBlockSize,
            //         outputBuffer, transformed);
            //
            // // last block adds padding which is required for decryption
            // var lastBytes =
            //     encryptTransform.TransformFinalBlock(plainText, transformed, plainText.Length - transformed);
            // outputBuffer.SetRange(transformed, lastBytes, 0, AesBlockSize);
            //
            // return outputBuffer;

            using (var plainStream = new MemoryStream(plainText, false))
            {
                using (var encryptTransform = _aes.CreateEncryptor())
                using (var cryptStream = new CryptoStream(plainStream, encryptTransform, CryptoStreamMode.Read))
                {
                    using var dest = new MemoryStream();
                    cryptStream.CopyTo(dest);

                    return dest.ToArray();
                }
            }
        }

        public byte[] DecryptBytes(byte[] cipherText)
        {
            // // create a buffer, the last two blocks are in the final buffer if the data is big enough
            // var buffer = cipherText.Length <= AesBlockSize
            //     ? new byte[AesBlockSize]
            //     : new byte[cipherText.Length - AesBlockSize * 2];
            //
            // _aes.Padding = PaddingMode.None;
            // 
            //
            // var transformed = 0;
            // while (transformed < cipherText.Length - AesBlockSize)
            // {
            //     // not incrementing by return value from TransformBlock here because
            //     // decrypt transform saves the first block and doesn't do anything on the first run
            //     // decrypt transform decrypts the saved block in the second run that's why
            //     // outputOffset is reduced by the aes block size to include the last block
            //     decryptTransform.TransformBlock(
            //         cipherText, transformed, AesBlockSize,
            //         buffer, transformed - AesBlockSize);
            //
            //     transformed += AesBlockSize;
            // }
            //
            // // transform final block removes padding
            // var finalBlock = decryptTransform.TransformFinalBlock(cipherText, transformed, AesBlockSize);
            //
            // // you only need to return the final block when the encrypted data smaller is than the block size
            // if (finalBlock.Length < AesBlockSize) return finalBlock;
            //
            // // create array with space for both buffer and last bytes
            // var output = new byte[buffer.Length + finalBlock.Length];
            // output.CombineFrom(buffer, finalBlock);
            //
            // return output;

            using (var cipherStream = new MemoryStream(cipherText, false))
            {
                using (var decryptor = _aes.CreateDecryptor())
                using (var cryptStream = new CryptoStream(cipherStream, decryptor, CryptoStreamMode.Read))
                {
                    using var dest = new MemoryStream();
                    cryptStream.CopyTo(dest);

                    return dest.ToArray();
                }
            }
        }
    }
}