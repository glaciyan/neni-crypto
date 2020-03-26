using System.Security.Cryptography;

namespace crypto.Core
{
    public static class CryptoRNG
    {
        public const int Aes256KeySizeInBytes = 32;
        public const int Aes256IvSizeInBytes = 16;
        
        public static byte[] GetRandomBytes(int length)
        {
            var randomBytes = new byte[length];

            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            return randomBytes;
        }
    }
}