using System;
using System.IO;
using System.Threading.Tasks;
using crypto.Core;
using crypto.Desktop.Cnsl.Recources;
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
            ToAddPath = addPath ?? throw new NullReferenceException(Strings.RenameCommand_RenameCommand_No_path_to_file_given);
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
                Notifier.Success(string.Format(Strings.AddCommandAsync_Run_Added_file__0__to_vault, ToAddPath));
            }
            else if (Directory.Exists(ToAddPath))
            {
                var progress = new Progress<ProgressReport>();
                progress.ProgressChanged += ProgressBar.PrintProgressBar;
                
                await AddDirectory(vault, progress);
            }
        }

        private async Task AddDirectory(Vault vault, IProgress<ProgressReport> progress)
        {
            var allFiles = NDirectory.GetAllFilesRecursive(ToAddPath);
            
            var report = new ProgressReport(allFiles.Count);

            await allFiles.ParallelForEachAsync(async file =>
            {
                var pathToFile = NPath.GetRelativePathToFile(ToAddPath, file);

                try
                {
                    await vault.AddFileAsync(file, pathToFile);
                }
                catch (Exception e)
                {
                    report.IncrementFailedFiles();
                    Log.Error(string.Format(Strings.AddCommandAsync_AddDirectory_Error_with_file__0____1_, file, e));
                }
                finally
                {
                    report.IncrementModifiedFiles();
                    progress.Report(report);
                }
            }, 0);

            Console.WriteLine();
            Notifier.Success(string.Format(Strings.AddCommandAsync_AddDirectory_Added_directory__0__to_vault, ToAddPath));
        }
    }
}