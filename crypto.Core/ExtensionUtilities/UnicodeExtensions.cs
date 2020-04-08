using System.Text;

namespace crypto.Core.ExtensionUtilities
{
    public static class UnicodeExtensions
    {
        public static string GetUnicodeString(this byte[] b)
        {
            return Encoding.Unicode.GetString(b);
        }

        public static byte[] GetUnicodeBytes(this string s)
        {
            return Encoding.Unicode.GetBytes(s);
        }
    }
}