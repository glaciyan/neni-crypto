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

        private CryptoMode _mode;
        
        public byte[] PasswordIV { get; }
        public byte[] PasswordAuthentication { get; }
        private byte[] Password { get; }

        public MasterPassword()
        {
            PasswordIV = CryptoRNG.GetRandomBytes(AesSizes.IV);
            Password = CryptoRNG.GetRandomBytes(AesSizes.Key);
            PasswordAuthentication = GeneratePasswordHash(Password);
            _mode = CryptoMode.Encryption;
        }

        public MasterPassword(byte[] passwordIV, byte[] passwordAuthentication, byte[] encryptedPassword)
        {
            PasswordIV = passwordIV;
            PasswordAuthentication = passwordAuthentication;
            Password = encryptedPassword;
            _mode = CryptoMode.Decryption;
        }

        public byte[] GetEncryptedPassword(byte[] key)
        {
            if (_mode == CryptoMode.Decryption)
                throw new InvalidOperationException("Password can't be encrypted when constructed for decryption");

            using var aes = new AesBytes(key, PasswordIV);

            return aes.EncryptBytes(Password);
        }

        public (bool, byte[]) GetDecryptedPassword(byte[] key)
        {
            if (_mode == CryptoMode.Encryption)
                throw new InvalidOperationException("Password can't be decrypted when constructed for encryption");
            
            using var aes = new AesBytes(key, PasswordIV);

            var decryptedPass = aes.DecryptBytes(Password);
            var hash = GeneratePasswordHash(decryptedPass);
            
            if (hash.ContentEqualTo(PasswordAuthentication))
            {
                return (true, decryptedPass);
            }
            
            throw new CryptographicException("Couldn't verify master password");
        }

        private static byte[] GeneratePasswordHash(byte[] password)
        {
            using var sha256 = SHA256.Create();

            var hash = new byte[0];
            for (var i = 0; i < Rounds; i++)
            {
                hash = sha256.ComputeHash(password);
            }

            return hash;
        }

        public void Dispose()
        {
            for (var i = 0; i < Password.Length; i++)
            {
                Password[i] = 0;
            }
        }
    }
}