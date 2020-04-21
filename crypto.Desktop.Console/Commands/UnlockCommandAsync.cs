using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using crypto.Core;
using crypto.Desktop.Cnsl.Recources;
using Dasync.Collections;
using Serilog;

namespace crypto.Desktop.Cnsl.Commands
{
    public class UnlockCommandAsync : CommandAsync
    {
        public UnlockCommandAsync(string? vaultPath)
        {
            VaultPath = vaultPath ?? Environment.CurrentDirectory;
        }

        private string? VaultPath { get; }

        public override async Task Run()
        {
            using var vault = StandardVault.Generate(VaultPath);

            var progress = new Progress<ProgressReport>();
            progress.ProgressChanged += ProgressBar.PrintProgressBar;
            
            var manipulatedFiles = await ExtractAllFiles(vault, progress);

            foreach (var manipulatedFile in manipulatedFiles)
                Notifier.Info(string.Format(Strings.UnlockCommandAsync_Run_File_has_been_altered, manipulatedFile.Header.SecuredPlainName.PlainName));

            Console.WriteLine();
            Notifier.Success(Strings.UnlockCommandAsync_Run_Vault_unlocked_);
        }

        private static async Task<List<UserDataFile>> ExtractAllFiles(Vault vlt, IProgress<ProgressReport> progress)
        {
            var manipulatedFiles = new List<UserDataFile>();

            var report = new ProgressReport(vlt.UserDataFiles.Count);
            
            await vlt.UserDataFiles.ParallelForEachAsync(async file =>
            {
                try
                {
                    var status = await vlt.ExtractFile(file);

                    if (status == ExtractStatus.HashNoMatch) manipulatedFiles.Add(file);
                }
                catch (Exception e)
                {
                    report.IncrementFailedFiles();
                    Log.Error(string.Format(Strings.UnlockCommandAsync_ExtractAllFiles_Error_unlocking_file__0____1_, file.Header.SecuredPlainName.PlainName, e));
                }
                finally
                {
                    report.IncrementModifiedFiles();
                    progress.Report(report);
                }
            }, 0);
            
            return manipulatedFiles;
        }
    }
}