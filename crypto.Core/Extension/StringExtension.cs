using System.Security.Cryptography;
using System.Text;

namespace crypto.Core.Extension
{
    public static class StringExtension
    {
        public static byte[] ApplySHA256(this string s)
        {
            using var sha256 = SHA256.Create();

            var hash = sha256.ComputeHash(Encoding.Unicode.GetBytes(s));

            return hash;
        }
    }
}