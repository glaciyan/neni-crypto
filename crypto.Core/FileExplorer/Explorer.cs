using System;
using System.Collections.Generic;

namespace crypto.Core.FileExplorer
{
    public class Explorer
    {
        public List<string[]> FilePaths { get; } = new List<string[]>();

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

        public Dictionary<string[], FileFolder> GetFromPath(string position)
        {
            var split = position.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var matchingFiles = new Dictionary<string[], FileFolder>();

            foreach (var path in FilePaths)
            {
                var matches = true;
                var i = 0;

                for (;i < split.Length; i++)
                {
                    matches = matches && split[i] == path[i];
                }

                var fileFolder = i == path.Length ? FileFolder.File : FileFolder.Folder;
                if (matches) matchingFiles.Add(path, fileFolder);
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

    public enum FileFolder
    {
        File,
        Folder
    }
}