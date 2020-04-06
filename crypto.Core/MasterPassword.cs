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
            EncryptedPassword = encryptedPassword;
            _mode = CryptoMode.Decryption;
        }

        public byte[] IV { get; }
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

            using var aes = new AesBytes(key, IV);

            return aes.EncryptBytes(Password);
        }

        public (bool, byte[]) GetDecryptedPassword(byte[] key)
        {
            if (_mode == CryptoMode.Encryption)
                throw new InvalidOperationException("Password can't be decrypted when constructed for encryption");

            using var aes = new AesBytes(key, IV);

            var decryptedPass = aes.DecryptBytes(EncryptedPassword);
            var hash = GeneratePasswordHash(decryptedPass);

            if (hash.ContentEqualTo(AuthenticationHash))
            {
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