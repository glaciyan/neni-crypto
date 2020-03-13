using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace crypto.Core
{
    public static class StringCryptography
    {
        public static byte[] EncryptString(string input, KeyIVPair keyRing)
        {
            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, Aes.Create().CreateEncryptor(keyRing.Key, keyRing.IV),
                CryptoStreamMode.Write);
            using var swEncrypt = new StreamWriter(csEncrypt);

            swEncrypt.Write(input);

            var encrypted = msEncrypt.ToArray();

            // TODO: returns nothing
            return encrypted;
        }

        public static string DecryptByteArray(byte[] input, KeyIVPair keyRing)
        {
            using var byteStream = new MemoryStream(input);
            using var cryptoStream = new CryptoStream(byteStream, Aes.Create().CreateDecryptor(keyRing.Key, keyRing.IV), CryptoStreamMode.Read);
            using var stringStream = new StreamReader(cryptoStream);

            var plain = stringStream.ReadToEnd();

            return plain;
        }
    }
}