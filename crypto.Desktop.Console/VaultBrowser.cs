using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using crypto.Core;
using crypto.Core.FileExplorer;
using crypto.Desktop.Cnsl.BrowserCommands;

namespace crypto.Desktop.Cnsl
{
    public enum ReturnStatus
    {
        Repeat,
        Stop
    }
    
    public class VaultBrowser
    {
        private readonly Vault _vault;
        public readonly Explorer Explorer = new Explorer();
        public readonly Stack<string> CurrentPathStack = new Stack<string>();
        
        public List<string> CurrentFolders = new List<string>();
        public List<string> CurrentFiles = new List<string>();
        
        public VaultBrowser(Vault vault)
        {
            _vault = vault;

            foreach (var header in vault.ItemHeaders)
            {
                Explorer.AddFile(header);
            }
        }

        private string[] CurrentPathArray => CurrentPathStack.Reverse().ToArray();

        public void Display()
        {
            UpdateFoldersAndFiles();
            foreach (var folder in CurrentFolders)
            {
                Console.WriteLine($"Folder -> {folder}");
            }

            foreach (var file in CurrentFiles)
            {
                Console.WriteLine($"File -> {file}");
            }
        }

        public void UpdateFoldersAndFiles()
        {
            var items = Explorer.GetFromPath(CurrentPathArray);
            CurrentFolders.Clear();
            CurrentFiles.Clear();
            
            foreach (var vaultItem in items)
            {
                var name = vaultItem.VaultItemWithSplitPath.SplitPath[vaultItem.Index];
                
                switch (vaultItem.Type)
                {
                    case FileFolder.File:
                        CurrentFiles.Add(name);
                        break;
                    case FileFolder.Folder:
                        CurrentFolders.Add(name);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void WaitForCommand()
        {
            while (true)
            {
                // go, open, move, delete, copy (implement for vault)
                Console.Write("> ");
                var consoleInput = Console.ReadLine();

                var args = new ArrayEnumerator<string>(consoleInput?.Split(' ', StringSplitOptions.RemoveEmptyEntries));

                var command = args.NextOrNull();

                if (command == null)
                {
                    Notifier.Error("No command entered.");
                    continue;
                }
                
                try
                {
                    var (cmd, status) = GetCommand(command, args);
                    cmd?.Run();

                    if (status == ReturnStatus.Repeat)
                    {
                        Display();
                        WaitForCommand();
                    }
                }
                catch (Exception e)
                {
                    Notifier.Error($"Something went wrong: {e.Message}");
                    WaitForCommand();
                }
                
                break;
            }
        }

        private (Command, ReturnStatus) GetCommand(string command, ArrayEnumerator<string> args)
        {
            switch (command)
            {
                case "q":
                case "Q":
                case "quit":
                    return (null, ReturnStatus.Stop);
                    
                case "cd":
                case "go":
                    var dest = args.NextOrNull();
                    return (new GoCommand(this, dest ?? throw new ArgumentException("No path given")),
                        ReturnStatus.Repeat);
            }
            
            throw new ArgumentException("No command given");
        }

        public void VerifiedPathPush(string path)
        {
            var paths = Explorer.GetFromPath(NPath.CombineArray(CurrentPathArray) + "/" + path);
            
            if (paths.Count == 0)
            {
                throw new DirectoryNotFoundException();
            }

            foreach (var goodPath in NPath.SplitPath(path))
            {
                CurrentPathStack.Push(goodPath);
            }
        }
    }
}