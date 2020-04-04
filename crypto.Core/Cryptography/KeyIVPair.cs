using System.Diagnostics.CodeAnalysis;

namespace crypto.Core.Cryptography
{
    public class KeyIVPair
    {
        public KeyIVPair()
        {
            Key = CryptoRNG.GetRandomBytes(AesSizes.Key);
            IV = CryptoRNG.GetRandomBytes(AesSizes.IV);
        }

        public KeyIVPair([NotNull] byte[] key, [NotNull] byte[] iv)
        {
            Key = key;
            IV = iv;
        }

        public byte[] Key { get; }

        public byte[] IV { get; }

        public static KeyIVPair FromPasswordString(string password, byte[] iv = null)
        {
            return
                new KeyIVPair(
                    password.ApplySHA256(),
                    iv ?? CryptoRNG.GetRandomBytes(AesSizes.IV)
                );
        }
    }
}