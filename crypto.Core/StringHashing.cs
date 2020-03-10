using System.Security.Cryptography;
using System.Text;

namespace crypto.Core
{
    public static class StringHashing
    {
        public static byte[] ToByteArraySHA256(this string s, int size)
        {
            using var sha256 = SHA256.Create();

            var hash = sha256.ComputeHash(Encoding.Unicode.GetBytes(s));

            return hash;
        }
    }
}