using System.Security.Cryptography;
using System.Text;

namespace crypto.Core
{
    public static class StringHashing
    {
        public static byte[] ToByteArraySHA256(this string s)
        {
            using var sha256 = SHA256.Create();

            var hash = sha256.ComputeHash(Encoding.Unicode.GetBytes(s));

            return hash;
        }
        
        public const int Rounds = 262143;

        public static byte[] StretchKey(this string passphrase)
        {
            var sha2 = SHA256.Create();
            var key = sha2.ComputeHash(passphrase.ToByteArraySHA256());

            for (var i = 0; i < Rounds; i++)
            {
                key = sha2.ComputeHash(key);
            }

            return key;
        }
    }
}