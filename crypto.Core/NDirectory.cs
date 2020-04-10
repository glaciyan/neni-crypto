using System;
using System.IO;
using System.Text;

namespace crypto.Core
{
    public static class NDirectory
    {
        public static void DeleteDirIfEmpty(string directory, string excludedName)
        {
            while (true)
            {
                var dirInfo = new DirectoryInfo(directory);

                if (dirInfo.GetDirectories().Length == 0 && dirInfo.GetFiles().Length == 0 && excludedName != dirInfo.Name)
                {
                    dirInfo.Delete();
                    if (dirInfo.Parent != null)
                    {
                        directory = dirInfo.Parent.FullName;
                        continue;
                    }
                }

                break;
            }
        }

        public static string GetPathParentDir(string path)
        {
            var split = path.Replace(Path.DirectorySeparatorChar, '/').Split('/', StringSplitOptions.RemoveEmptyEntries);
            var output = new StringBuilder();
            if (path[0] == '/') output.Append('/');
            
            for (var i = 0; i < split.Length - 1; i++)
            {
                output.Append(split[i] + '/');
            }

            return output.ToString();
        }
    }
}