using System;
using System.Text;

namespace crypto.Core
{
    public static class NPath
    {
        public static string RemoveRelativeParts(string path)
        {
            var split = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var result = new StringBuilder();

            for (var i = 0; i < split.Length; i++)
            {
                if (split[i] == ".." || split[i] == ".") continue;
                
                result.Append(split[i]);
               
                if (i < split.Length - 1)
                {
                    result.Append('/');
                }
            }

            return result.ToString();
        }
    }
}