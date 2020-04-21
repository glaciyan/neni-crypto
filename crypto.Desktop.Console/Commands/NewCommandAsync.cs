using System;
using System.IO;
using System.Threading.Tasks;
using crypto.Core;
using crypto.Desktop.Cnsl.Recources;
using Serilog;

namespace crypto.Desktop.Cnsl.Commands
{
    public class NewCommandAsync : CommandAsync
    {
        /* new -> uses current directory
         * new name -> creates a new directory with that name and uses that
         * new name path -> goes to that path and creates a new directory with the given name and uses that
         */

        public NewCommandAsync(string? name, string? path)
        {
            Name = name;
            Path = path;
        }

        private string? Name { get; }
        private string? Path { get; }

        public override async Task Run()
        {
            await Task.Run(() =>
            {
                Log.Debug("Running NewProject with " +
                          $"Name = {Name ?? "null"}, Path = {Path ?? "null"}");

                var vaultName = Name ?? GetCurrentDirectoryName();
                var vaultPath = GetVaultCtorPath(Path);
                var folderPath = GetFolderPath(vaultName, Path);

                Log.Debug($"vaultPath: {folderPath}");

                if (!NDirectory.IsDirectoryEmpty(folderPath))
                    throw new DirectoryNotEmptyException(Strings.NewCommandAsync_Run_The_directory_vault_is_not_empty);

                var key = PasswordPrompt.PromptPasswordWithConfirmationAsHash();

                using var vault = Vault.Create(vaultName, key, vaultPath);

                Notifier.Success(string.Format(Strings.NewCommandAsync_Run_Created_vault__0__as__1__, vaultName, folderPath));
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
                return currDir.Parent?.FullName ?? currDir.FullName;

            throw new DirectoryNotEmptyException(Strings.NewCommandAsync_Run_The_directory_vault_is_not_empty);
        }

        private static string GetCurrentDirectoryName()
        {
            var currentDir = new DirectoryInfo(Environment.CurrentDirectory);
            return currentDir.Name;
        }
    }
}