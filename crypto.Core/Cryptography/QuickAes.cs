using System.Security.Cryptography;
using crypto.Core.Header;
// ReSharper disable PossibleNullReferenceException

namespace crypto.Core.Cryptography
{
    public class QuickAes
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