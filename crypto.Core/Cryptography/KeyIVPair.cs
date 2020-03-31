using System.Diagnostics.CodeAnalysis;

namespace crypto.Core.Cryptography
{
    public class KeyIVPair
    {
        public KeyIVPair()
        {
            Key = CryptoRNG.GetRandomBytes(CryptoRNG.Aes256KeySizeInBytes);
            IV = CryptoRNG.GetRandomBytes(CryptoRNG.Aes256IvSizeInBytes);
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
                    password.ToByteArraySHA256(),
                    iv ?? CryptoRNG.GetRandomBytes(CryptoRNG.Aes256IvSizeInBytes)
                );
        }
    }
}