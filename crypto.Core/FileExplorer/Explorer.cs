using System;
using System.Collections.Generic;

namespace crypto.Core.FileExplorer
{
    public enum FileFolder
    {
        File,
        Folder
    }
    
    public class Explorer
    {
        private List<string[]> FilePaths { get; } = new List<string[]>();

        public Explorer(params string[] files)
        {
            foreach (var path in files)
            {
                AddFile(path);
            }
        }

        public void AddFile(string path)
        {
            FilePaths.Add(path.Replace('\\', '/').Split('/', StringSplitOptions.RemoveEmptyEntries));
        }

        public IEnumerable<(string[], FileFolder, int)> GetFromPath(string position)
        {
            var split = position.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var matchingFiles = new List<(string[], FileFolder, int)>();

            foreach (var path in FilePaths)
            {
                if (split.Length == 0 && path.Length == 1)
                {
                    matchingFiles.Add((path, FileFolder.File, 0));
                    continue;
                }
                
                if (split.Length == path.Length)
                    throw new ArgumentException("path is pointing to file");
                
                var matches = true;
                var i = 0;
                for (;i < split.Length; i++)
                {
                    if (!matches) break;
                    matches = split[i] == path[i];
                }
                
                if (matches)
                {
                    var fileFolder = i == path.Length ? FileFolder.File : FileFolder.Folder;
                    matchingFiles.Add((path, fileFolder, i));
                }
            }

            return matchingFiles;
        }

        private static IEnumerable<T> CopyList<T>(List<T> list)
        {
            var result = new List<T>(list.Capacity);

            foreach (var item in list)
            {
                result.Add(item);
            }

            return result;
        }
    }
}