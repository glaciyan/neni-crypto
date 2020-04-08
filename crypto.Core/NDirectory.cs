using System;
using System.IO;
using System.Text;

namespace crypto.Core
{
    public static class NDirectory
    {
        public static void CleanEmptyDirectories(string root)
        {
            var dirInfo = new DirectoryInfo(root);

            if (dirInfo.GetDirectories().Length == 0)
            {
                dirInfo.Delete();
                return;
            }
            
            RecursiveCleanUp(dirInfo);
        }

        private static void RecursiveCleanUp(DirectoryInfo dirInfo)
        {
            foreach (var directoryInfo in dirInfo.EnumerateDirectories())
            {
                var dirs = directoryInfo.GetDirectories();

                if (dirs.Length == 0) directoryInfo.Delete();
                else
                {
                    RecursiveCleanUp(directoryInfo);
                }
            }
        }
        
        public static string GetPathToFile(string filePath)
        {
            var split = filePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var output = new StringBuilder();

            for (var i = 0; i < split.Length - 1; i++)
            {
                output.Append(split[i]);
                
                if (i < split.Length - 2)
                {
                    output.Append('/');
                }
            }

            return output.ToString();
        }
    }
}