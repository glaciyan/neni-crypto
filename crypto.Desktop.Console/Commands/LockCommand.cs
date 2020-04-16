using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using crypto.Core;
using Dasync.Collections;
using Serilog;

namespace crypto.Desktop.Cnsl.Commands
{
    public class LockCommand : CommandAsync
    {
        public LockCommand(string? vaultPath)
        {
            VaultPath = vaultPath ?? Environment.CurrentDirectory;
        }

        private string? VaultPath { get; }

        public override async Task Run()
        {
            var key = PasswordPrompt.PromptPasswordAsHash();
            
            var paths = new VaultPaths(VaultPath);
            using var vault = Vault.Open(paths, key);

            await LockAllFiles(vault);
            
            Notifier.Success("\nLocked all files in Vault.");
        }

        private async Task LockAllFiles(Vault vault)
        {
            await vault.UserDataFiles.ParallelForEachAsync(async file =>
            {
                var encryptedFi = new FileInfo(vault.UserDataPathToEncrypted(file));
                var unlockedFi = new FileInfo(vault.UserDataPathToUnlocked(file));

                try
                {
                    if (encryptedFi.LastWriteTime == unlockedFi.LastWriteTime)
                    {
                        await vault.EliminateExtracted(file);
                    }
                    else
                    {
                        var modFile = new ModifiedUserDataFile(file, unlockedFi.FullName, encryptedFi.FullName);
                        Log.Information("Updating file " + modFile.UnlockedFilePath);
                        await vault.WriteDecryptedAsync(modFile.UserDataFile, modFile.UnlockedFilePath,
                            modFile.EncryptedFilePath);

                        await vault.EliminateExtracted(modFile.UserDataFile);
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"Error locking file {file.Header.SecuredPlainName.PlainName}: {e}");
                }
            }, 0);
        }
    }
}