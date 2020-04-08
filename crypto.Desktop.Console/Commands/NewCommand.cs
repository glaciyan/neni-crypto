using System;
using System.IO;
using crypto.Core;
using crypto.Core.Extension;
using Serilog;

namespace crypto.Desktop.Cnsl.Commands
{
    public class NewCommand : Command
    {
        public NewCommand(string? name, string? path)
        {
            Name = name;
            Path = path;
        }
        // new -> uses current directory
        // new name -> creates a new directory with that name and uses that
        // new name path -> goes to that path and creates a new directory with the given name and uses that

        private string? Name { get; }
        private string? Path { get; }

        public override void Run()
        {
            Log.Debug("Running NewProject with " +
                      $"Name = {Name ?? "null"}, Path = {Path ?? "null"}");

            var key = PasswordPrompt.PromptPasswordWithConfirmation().ApplySHA256();

            var vaultName = Name ?? GetCurrentDirectoryName();
            using var vault = Vault.Create(vaultName, key, Path);
            
            Notifier.Success("Created vault " + vaultName);
        }

        private static string GetCurrentDirectoryName()
        {
            var currentDir = new DirectoryInfo(Environment.CurrentDirectory);
            return currentDir.Name;
        }
    }
}