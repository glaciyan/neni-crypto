using System;
using System.IO;
using System.Text;

namespace crypto.Core
{
    public static class NPath
    {
        public static string RemoveRelativeParts(string path)
        {
            var split = SplitPath(path.Replace('\\', '/'));
            var result = new StringBuilder();

            for (var i = 0; i < split.Length; i++)
            {
                if (split[i] == ".." || split[i] == ".") continue;

                result.Append(split[i]);

                if (i < split.Length - 1) result.Append('/');
            }

            return result.ToString();
        }

        public static string CombineArray(string[] arr)
        {
            var result = new StringBuilder();

            for (var index = 0; index < arr.Length; index++)
            {
                var str = arr[index];
                result.Append(str);

                if (index < arr.Length - 1) result.Append('/');
            }

            return result.ToString();
        }

        public static string GetRelativePathToFile(string relativeTo, string path)
        {
            var relativePath = Path.GetRelativePath(relativeTo + "/..", path);

            return NDirectory.GetPathParentDir(relativePath);
        }

        public static string[] SplitPath(string path)
        {
            return path.Replace('\\', '/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}