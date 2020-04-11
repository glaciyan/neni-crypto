using System;
using crypto.Core;
using crypto.Core.FileExplorer;

namespace crypto.Desktop.Cnsl
{
    public class VaultBrowser
    {
        private readonly Vault _vault;
        private readonly Explorer _explorer = new Explorer();

        public VaultBrowser(Vault vault)
        {
            _vault = vault;

            foreach (var header in vault.ItemHeaders)
            {
                _explorer.AddFile(header);
            }
        }

        public void Display(string path)
        {
            var items = _explorer.GetFromPath(path);
            foreach (var (splitPath, fileFolder, nameIndex) in items)
            {
                switch (fileFolder)
                {
                    case FileFolder.File:
                        Console.Write("File -> ");
                        break;
                    case FileFolder.Folder:
                        Console.Write("Folder -> ");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Console.WriteLine(splitPath.SplitPath[nameIndex]);
            }
        }

        
    }
}