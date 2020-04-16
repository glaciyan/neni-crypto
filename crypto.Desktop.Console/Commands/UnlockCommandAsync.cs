using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using crypto.Core;
using crypto.Core.Extension;
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
            var key = PasswordPrompt.PromptPassword().ApplySHA256();

            var paths = new VaultPaths(VaultPath);
            using var vault = Vault.Open(paths, key);

            var manipulatedFiles = await ExtractAllFiles(vault);

            foreach (var manipulatedFile in manipulatedFiles)
            {
                Notifier.Error($"File: {manipulatedFile.Header.SecuredPlainName.PlainName} " +
                               "has been altered, be careful with this file");
            }
            
            Notifier.Success("Vault unlocked.");
        }

        private static async Task<List<UserDataFile>> ExtractAllFiles(Vault vlt)
        {
            var manipulatedFiles = new List<UserDataFile>();
        
            foreach (var file in vlt.UserDataFiles)
            {
                try
                {
                    var status = await vlt.ExtractFile(file);

                    if (status == ExtractStatus.HashNoMatch)
                    {
                        manipulatedFiles.Add(file);
                    }
                }
                catch (Exception e)
                {
                    Notifier.Error($"Error unlocking file {file.Header.SecuredPlainName.PlainName}: {e.Message}");
                    Log.Error(e.ToString());
                    throw;
                }
            }
        
            return manipulatedFiles;
        }
    }
}