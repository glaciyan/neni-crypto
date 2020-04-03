using System.Security.Cryptography;

namespace crypto.Core.Cryptography
{
    public static class CryptoRNG
    {
        public static byte[] GetRandomBytes(int length)
        {
            var randomBytes = new byte[length];

            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            return randomBytes;
        }
    }
}