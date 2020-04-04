using System;
using System.Security.Cryptography;
using crypto.Core.Cryptography;

namespace crypto.Core
{
    public enum CryptoMode
    {
        Encryption,
        Decryption
    }

    public class MasterPassword : IDisposable
    {
        private const int Rounds = 65536;

        private readonly CryptoMode _mode;

        public MasterPassword()
        {
            IV = CryptoRNG.GetRandomBytes(AesSizes.IV);
            Password = CryptoRNG.GetRandomBytes(AesSizes.Key);
            AuthenticationHash = GeneratePasswordHash(Password);
            _mode = CryptoMode.Encryption;
        }

        public MasterPassword(byte[] iv, byte[] authenticationHash, byte[] encryptedPassword)
        {
            IV = iv;
            AuthenticationHash = authenticationHash;
            Password = encryptedPassword;
            _mode = CryptoMode.Decryption;
        }

        public byte[] IV { get; set; }
        public byte[] AuthenticationHash { get; set; }
        private byte[] Password { get; }

        public void Dispose()
        {
            for (var i = 0; i < Password.Length; i++) Password[i] = 0;
        }

        public byte[] GetEncryptedPassword(byte[] key)
        {
            if (_mode == CryptoMode.Decryption)
                throw new InvalidOperationException("Password can't be encrypted when constructed for decryption");

            using var aes = new AesBytes(key, IV);

            return aes.EncryptBytes(Password);
        }

        public (bool, byte[]) GetDecryptedPassword(byte[] key)
        {
            if (_mode == CryptoMode.Encryption)
                throw new InvalidOperationException("Password can't be decrypted when constructed for encryption");

            using var aes = new AesBytes(key, IV);

            var decryptedPass = aes.DecryptBytes(Password);
            var hash = GeneratePasswordHash(decryptedPass);

            if (hash.ContentEqualTo(AuthenticationHash)) return (true, decryptedPass);

            throw new CryptographicException("Couldn't verify master password");
        }

        private static byte[] GeneratePasswordHash(byte[] password)
        {
            using var sha256 = SHA256.Create();

            var hash = new byte[0];
            for (var i = 0; i < Rounds; i++) hash = sha256.ComputeHash(password);

            return hash;
        }
    }
}