using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using crypto.Core;
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
            progress.ProgressChanged += FilesProgressBar.PrintProgressBar;
            
            var manipulatedFiles = await ExtractAllFiles(vault, progress);

            foreach (var manipulatedFile in manipulatedFiles)
                Notifier.Info($"File: {manipulatedFile.Header.SecuredPlainName.PlainName} " +
                               "has been altered, be careful with this file");

            Notifier.Success("\nVault unlocked.");
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
                    Log.Error($"Error unlocking file {file.Header.SecuredPlainName.PlainName}: {e}");
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