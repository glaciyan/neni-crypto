using System.Diagnostics.CodeAnalysis;

namespace crypto.Core
{
    public class KeyIVPair
    {
        public KeyIVPair()
        {
            Key = CryptoRNG.GetRandomBytes(32);
            IV = CryptoRNG.GetRandomBytes(16);
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
                    password.ToByteArraySHA256(32),
                    iv ?? CryptoRNG.GetRandomBytes(16)
                );
        }
    }
}