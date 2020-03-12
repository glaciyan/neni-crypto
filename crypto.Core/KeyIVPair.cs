using System.Diagnostics.CodeAnalysis;

namespace crypto.Core
{
    public class KeyIVPair
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public byte[] Key => _key;
        public byte[] IV => _iv;

        public KeyIVPair([NotNull] byte[] key, [NotNull] byte[] iv)
        {
            _key = key;
            _iv = iv;
        }

        public static KeyIVPair FromPasswordString(string password, byte[] iv = null)
        {
            return
                new KeyIVPair(
                    password.ToByteArraySHA256(32),
                    iv ?? CryptoRNG.GetRandomByte(16)
                );
        }
    }
}