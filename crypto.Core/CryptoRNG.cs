using System.Security.Cryptography;

namespace crypto.Core
{
    public static class CryptoRNG
    {
        public static byte[] GetRandomByte(int length)
        {
            var randomBytes = new byte[length];

            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            return randomBytes;
        }
    }
}