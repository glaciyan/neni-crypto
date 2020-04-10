using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public static IEnumerable<string> GetAllFilesRecursive(string path)
        {
            var allFilePaths = new List<string>();
            var dirInfo = new DirectoryInfo(path);
            
            foreach (var file in dirInfo.EnumerateFiles())
            {
                allFilePaths.Add(file.FullName);
            }

            foreach (var directory in dirInfo.EnumerateDirectories())
            {
                allFilePaths.AddRange(GetAllFilesRecursive(directory.FullName));
            }

            return allFilePaths;
        }
        
        public static bool IsDirectoryEmpty(string dirPath)
        {
            if (dirPath == null) return true;
            
            var dirInfo = new DirectoryInfo(dirPath);
            if (!dirInfo.Exists) return true;
            
            return dirInfo.GetDirectories().Length == 0 && dirInfo.GetFiles().Length == 0;
        }
    }
}