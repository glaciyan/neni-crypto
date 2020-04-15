using System;
using System.Security.Cryptography;
using crypto.Core.Extension;

namespace crypto.Core.Cryptography
{
    public enum CryptoMode
    {
        Encryption,
        Decryption
    }

    public class MasterPassword : IDisposable
    {
        private const int Rounds = 65536;

        private CryptoMode _mode;

        public MasterPassword()
        {
            Password = CryptoRNG.GetRandomBytes(AesSizes.Key);
            AuthenticationHash = GeneratePasswordHash(Password);
            _mode = CryptoMode.Encryption;
        }

        public MasterPassword(byte[] authenticationHash, byte[] encryptedPassword)
        {
            AuthenticationHash = authenticationHash;
            EncryptedPassword = encryptedPassword;
            _mode = CryptoMode.Decryption;
        }

        public byte[] AuthenticationHash { get; }
        public byte[] Password { get; set; }
        private byte[] EncryptedPassword { get; }

        public void Dispose()
        {
            for (var i = 0; i < Password.Length; i++) Password[i] = 0;
        }

        public byte[] GetEncryptedPassword(byte[] key)
        {
            if (_mode == CryptoMode.Decryption)
                throw new InvalidOperationException("Password can't be encrypted when constructed for decryption");

            return key.Xor(Password);
        }

        public (bool, byte[]) GetDecryptedPassword(byte[] key)
        {
            if (_mode == CryptoMode.Encryption)
                throw new InvalidOperationException("Password can't be decrypted when constructed for encryption");

            var decryptedPass = key.Xor(EncryptedPassword);
            var hash = GeneratePasswordHash(decryptedPass);

            if (hash.ContentEqualTo(AuthenticationHash))
            {
                _mode = CryptoMode.Encryption;
                Password = decryptedPass;
                return (true, decryptedPass);
            }

            return (false, new byte[0]);
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