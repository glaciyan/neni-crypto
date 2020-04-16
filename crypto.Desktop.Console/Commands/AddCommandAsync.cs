using System;
using System.IO;
using System.Threading.Tasks;
using crypto.Core;
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
                var progress = new Progress<ProgressReport>();
                progress.ProgressChanged += FilesProgressBar.PrintProgressBar;
                
                await AddDirectory(vault, progress);
            }
        }

        private async Task AddDirectory(Vault vault, IProgress<ProgressReport> progress)
        {
            var allFiles = NDirectory.GetAllFilesRecursive(ToAddPath);
            
            var report = new ProgressReport(allFiles.Count);

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
                    report.IncrementFailedFiles();
                    Log.Error($"Error with file {file}: {e}");
                }
                finally
                {
                    report.IncrementModifiedFiles();
                    progress.Report(report);
                }
            }, 0);

            Notifier.Success($"\nAdded directory {ToAddPath} to vault");
        }
    }
}