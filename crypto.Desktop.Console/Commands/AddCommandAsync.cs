using System;
using System.IO;
using System.Threading.Tasks;
using crypto.Core;
using crypto.Core.Extension;
using Serilog;

namespace crypto.Desktop.Cnsl.Commands
{
    public class AddCommandAsync : CommandAsync
    {
        private string? ToAddPath { get; }
        private string? VaultPath { get; }

        /* add -> error missing file
         * add fileName -> look is current dir is a vault then add that file if true
         * add fileName vaultPath -> look if the given dir is a vault then add it if true
         */
        
        public AddCommandAsync(string? vaultPath, string? addPath)
        {
            ToAddPath = addPath ?? throw new NullReferenceException("Path to file not given");
            VaultPath = vaultPath;
        }

        public override async Task Run()
        {
            VaultReadingPaths paths;
            if (VaultPath == null)
            {
                paths = new VaultReadingPaths(Environment.CurrentDirectory);
            }
            else
            {
                paths = new VaultReadingPaths(VaultPath);
            }
            
            var key = PasswordPrompt.PromptPassword().ApplySHA256();
            using var vault = Vault.Open(paths, key);

            if (File.Exists(ToAddPath))
            {
                await vault.AddFileAsync(ToAddPath);
                Notifier.Success($"Added file {ToAddPath} to vault");
            }
            else if (Directory.Exists(ToAddPath))
            {
                foreach (var file in NDirectory.GetAllFilesRecursive(ToAddPath))
                {
                    var pathToFile = NPath.GetRelativePathToFile(ToAddPath, file);
                    await vault.AddFileAsync(file, pathToFile);
                }
                Notifier.Success($"Added directory {ToAddPath} to vault");
            }
            
            Log.Debug("Added file to vault");
        }
    }
}