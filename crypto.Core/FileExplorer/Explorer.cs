using System;
using System.Collections.Generic;
using crypto.Core.Header;

namespace crypto.Core.FileExplorer
{
    public enum FileFolder
    {
        File,
        Folder
    }
    
    public class Explorer
    {
        private List<ExplorableVaultItemPath> ItemHeaders { get; } = new List<ExplorableVaultItemPath>();

        public Explorer(params ItemHeader[] headers)
        {
            foreach (var path in headers)
            {
                AddFile(path);
            }
        }

        public void AddFile(ItemHeader header)
        {
            ItemHeaders.Add(new ExplorableVaultItemPath(header));
        }

        public IEnumerable<(ExplorableVaultItemPath, FileFolder, int)> GetFromPath(string position)
        {
            var split = position.Split('/', StringSplitOptions.RemoveEmptyEntries);
            return GetFromPath(split);
        }
        
        public IEnumerable<(ExplorableVaultItemPath, FileFolder, int)> GetFromPath(string[] split)
        {
            var matchingFiles = new List<(ExplorableVaultItemPath, FileFolder, int)>();

            foreach (var item in ItemHeaders)
            {
                var path = item.SplitPath;
                if (split.Length == 0 && path.Length == 1)
                {
                    matchingFiles.Add((item, FileFolder.File, 0));
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
                    matchingFiles.Add((item, fileFolder, i));
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