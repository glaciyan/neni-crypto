using System;
using System.IO;
using System.Threading.Tasks;
using crypto.Core;
using crypto.Core.Extension;
using Serilog;

namespace crypto.Desktop.Cnsl.Commands
{
    public class NewCommandAsync : CommandAsync
    {
        private string? Name { get; }
        private string? Path { get; }
        
        /* new -> uses current directory
         * new name -> creates a new directory with that name and uses that
         * new name path -> goes to that path and creates a new directory with the given name and uses that
         */
        
        public NewCommandAsync(string? name, string? path)
        {
            Name = name;
            Path = path;
        }
        
        public override async Task Run()
        {
            await Task.Run(() =>
            {
                Log.Debug("Running NewProject with " +
                          $"Name = {Name ?? "null"}, Path = {Path ?? "null"}");

                string vaultName = Name ?? GetCurrentDirectoryName();
                var vaultPath = GetVaultCtorPath(Path);
                var folderPath = GetFolderPath(vaultName, Path);

                Log.Debug($"vaultPath: {folderPath}");
                
                if (!NDirectory.IsDirectoryEmpty(folderPath))
                    throw new DirectoryNotEmptyException("The directory with the vault name is not empty");
                
                var key = PasswordPrompt.PromptPasswordWithConfirmation().ApplySHA256();

                using var vault = Vault.Create(vaultName, key, vaultPath);

                Notifier.Success($"Created vault {vaultName} as {folderPath}.");
            });
        }

        private static string GetFolderPath(string vaultName, string? path)
        {
            if (path == null) return System.IO.Path.Combine(Environment.CurrentDirectory, vaultName);
            return System.IO.Path.Combine(path, vaultName);
        }

        private static string? GetVaultCtorPath(string? path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            var currDir = new DirectoryInfo(Environment.CurrentDirectory);

            if (currDir.GetDirectories().Length == 0 && currDir.GetFiles().Length == 0)
            {
                return currDir.Parent?.FullName ?? currDir.FullName;
            }
            
            throw new DirectoryNotEmptyException("The directory is not empty, can't create vault");
        }

        private static string GetCurrentDirectoryName()
        {
            var currentDir = new DirectoryInfo(Environment.CurrentDirectory);
            return currentDir.Name;
        }
    }
}