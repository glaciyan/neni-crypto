using System;
using System.IO;
using System.Threading.Tasks;
using crypto.Core;
using crypto.Core.Extension;
using Dasync.Collections;
using Serilog;

namespace crypto.Desktop.Cnsl.Commands
{
    public class AddCommandAsync : CommandAsync
    {
        /* add -> error missing file
         * add fileName -> look is current dir is a vault then add that file if true
         * add fileName vaultPath -> look if the given dir is a vault then add it if true
         */

        public AddCommandAsync(string? vaultPath, string? addPath)
        {
            ToAddPath = addPath ?? throw new NullReferenceException("Path to file not given");
            VaultPath = vaultPath;
        }

        private string? ToAddPath { get; }
        private string? VaultPath { get; }

        public override async Task Run()
        {
            VaultPaths paths;
            if (VaultPath == null)
                paths = new VaultPaths(Environment.CurrentDirectory);
            else
                paths = new VaultPaths(VaultPath);

            var key = PasswordPrompt.PromptPasswordAsHash();
            using var vault = Vault.Open(paths, key);

            if (File.Exists(ToAddPath))
            {
                await vault.AddFileAsync(ToAddPath);
                Notifier.Success($"Added file {ToAddPath} to vault");
            }
            else if (Directory.Exists(ToAddPath))
            {
                var allFiles = NDirectory.GetAllFilesRecursive(ToAddPath);

                await allFiles.ParallelForEachAsync(async file =>
                {
                    Log.Debug($"Adding file: {file}, with size {new FileInfo(file).Length}");
                    var pathToFile = NPath.GetRelativePathToFile(ToAddPath, file);

                    try
                    {
                        await vault.AddFileAsync(file, pathToFile);
                    }
                    catch (Exception e)
                    {
                        Notifier.Error($"Error with file {file}: {e.Message}");
                    }
                }, 0);

                Notifier.Success($"Added directory {ToAddPath} to vault");
            }
        }
    }
}