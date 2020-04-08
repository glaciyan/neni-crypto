using System;
using System.Text;

namespace crypto.Core.ExtensionUtilities
{
    public class RandomGenerator
    {
        public static string RandomFileName(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_".ToCharArray();

            var eng = new Random();
            var s = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var index = eng.Next(0, chars.Length);
                s.Append(chars[index]);
            }

            return s.ToString();
        }
    }
}