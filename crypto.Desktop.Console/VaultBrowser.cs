using System;
using System.Collections.Generic;
using System.Linq;
using crypto.Core;
using crypto.Core.FileExplorer;

namespace crypto.Desktop.Cnsl
{
    public class VaultBrowser
    {
        private readonly Vault _vault;
        private readonly Explorer _explorer = new Explorer();
        public readonly Stack<string> CurrentPathStack = new Stack<string>();
        
        public VaultBrowser(Vault vault)
        {
            _vault = vault;

            foreach (var header in vault.ItemHeaders)
            {
                _explorer.AddFile(header);
            }
        }

        private string[] CurrentPathArray => CurrentPathStack.Reverse().ToArray();
        public ExplorableVaultItem[] GetFromPath =>
            (ExplorableVaultItem[]) _explorer.GetFromPath(CurrentPathArray);

        public void Display()
        {
            var items = GetFromPath;
            foreach (var vaultItem in items)
            {
                switch (vaultItem.Type)
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

                Console.WriteLine(vaultItem.VaultItemWithSplitPath.SplitPath[vaultItem.Index]);
            }
        }

        public void WaitForCommand()
        {
            Console.Write("> ");
            var args = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}