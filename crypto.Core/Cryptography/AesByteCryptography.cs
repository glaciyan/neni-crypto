using System;
using System.Security.Cryptography;
using System.Text;

namespace crypto.Core.Cryptography
{
    public class AesByteCryptography : IDisposable
    {
        private const int AesBlockSize = 16;
        private readonly Aes _aes;
        private readonly KeyIVPair _keyIvPair;

        public AesByteCryptography(KeyIVPair keyIvPair)
        {
            _keyIvPair = keyIvPair;
            
            // create aes with preferred settings
            _aes = Aes.Create();
            _aes.KeySize = 256;
            _aes.Key = _keyIvPair.Key;
            _aes.IV = _keyIvPair.IV;
            _aes.Padding = PaddingMode.PKCS7;
        }
        
        public byte[] EncryptBytes(byte[] plainText)
        {
            // get the size with spacing for padding
            var outputSize = plainText.Length + (AesBlockSize - (plainText.Length % AesBlockSize));
            var outputBuffer = new byte[outputSize];

            using var encryptTransform = _aes.CreateEncryptor();

            // counts the transformed bytes
            var transformed = 0;
            while (transformed < plainText.Length - AesBlockSize)
            {
                transformed += encryptTransform.TransformBlock(
                    plainText, transformed, AesBlockSize,
                    outputBuffer, transformed);
            }

            // last block adds padding which is required for decryption
            var lastBytes = encryptTransform.TransformFinalBlock(plainText, transformed, plainText.Length - transformed);
            outputBuffer.SetRange(transformed, lastBytes, 0, AesBlockSize);

            return outputBuffer;
        }

        public byte[] DecryptBytes(byte[] cipherText)
        {
            // create a buffer, the last two blocks are in the final buffer if the data is big enough
            var buffer = cipherText.Length <= AesBlockSize ?
                new byte[AesBlockSize] : new byte[cipherText.Length - AesBlockSize * 2];

            using var decryptTransform = _aes.CreateDecryptor();
            
            var transformed = 0;
            while (transformed < cipherText.Length - AesBlockSize)
            {
                // not incrementing by return value from TransformBlock here because
                // decrypt transform saves the first block and doesn't do anything on the first run
                // decrypt transform decrypts the saved block in the second run that's why
                // outputOffset is reduced by the aes block size to include the last block
                decryptTransform.TransformBlock(
                    cipherText, transformed, AesBlockSize, 
                    buffer, transformed - AesBlockSize);
                
                transformed += AesBlockSize;
            }

            // transform final block removes padding
            var finalBlock = decryptTransform.TransformFinalBlock(cipherText, transformed, AesBlockSize);

            // you only need to return the final block when the encrypted data smaller is than the block size
            if (finalBlock.Length < AesBlockSize) return finalBlock;
            
            // create array with space for both buffer and last bytes
            var output = new byte[buffer.Length + finalBlock.Length];
            output.MergeFrom(buffer, finalBlock);
            
            return output;
        }

        public void Dispose()
        {
            _aes?.Dispose();
        }
    }
}