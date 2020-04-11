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
        private List<VaultItemWithSplitPath> ItemHeaders { get; } = new List<VaultItemWithSplitPath>();

        public Explorer(params ItemHeader[] headers)
        {
            foreach (var path in headers)
            {
                AddFile(path);
            }
        }

        public void AddFile(ItemHeader header)
        {
            ItemHeaders.Add(new VaultItemWithSplitPath(header));
        }

        public IEnumerable<ExplorableVaultItemTypeIndex> GetFromPath(string position)
        {
            var split = NPath.SplitPath(position);
            return GetFromPath(split);
        }
        
        public IEnumerable<ExplorableVaultItemTypeIndex> GetFromPath(string[] split)
        {
            var matchingFiles = new List<ExplorableVaultItemTypeIndex>();
            var folders = new List<string>();
            
            foreach (var item in ItemHeaders)
            {
                var path = item.SplitPath;
                if (split.Length == 0 && path.Length == 1)
                {
                    matchingFiles.Add(new ExplorableVaultItemTypeIndex(item, FileFolder.File, 0));
                    continue;
                }
                
                if (split.Length == path.Length)
                    throw new ArgumentException("Path is pointing to file");
                
                var matches = true;
                var i = 0;
                for (;i < split.Length; i++)
                {
                    if (!matches) break;
                    matches = split[i] == path[i];
                }
                
                if (matches)
                {
                    var fileFolder = i == path.Length - 1 ? FileFolder.File : FileFolder.Folder;
                    
                    if (fileFolder == FileFolder.Folder)
                    {
                        if (folders.Contains(item.SplitPath[i]))
                        {
                            continue;
                        }
                        
                        folders.Add(item.SplitPath[i]);
                    }
                    
                    matchingFiles.Add(new ExplorableVaultItemTypeIndex(item, fileFolder, i));
                }
            }

            return matchingFiles;
        }

/*
        private static IEnumerable<T> CopyList<T>(List<T> list)
        {
            var result = new List<T>(list.Capacity);

            foreach (var item in list)
            {
                result.Add(item);
            }

            return result;
        }
*/
    }
}