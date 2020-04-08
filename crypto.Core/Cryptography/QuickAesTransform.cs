using System.Security.Cryptography;

// ReSharper disable PossibleNullReferenceException

namespace crypto.Core.Cryptography
{
    public static class QuickAesTransform
    {
        public static ICryptoTransform CreateEncryptor(byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Key = key;
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;
            var encryptor = aes.CreateEncryptor();
            return encryptor;
        }

        public static ICryptoTransform CreateDecryptor(byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Key = key;
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;
            var decryptor = aes.CreateDecryptor();
            return decryptor;
        }
    }
}