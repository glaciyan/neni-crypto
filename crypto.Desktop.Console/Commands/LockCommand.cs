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

            var modifiedFiles = await EliminateAllUnmodifiedFiles(vault);
            await UpdateThenEliminateModifiedFiles(vault, modifiedFiles);
        }
        
        //await vault.WriteDecryptedAsync(file, unlockedFi.FullName, encryptedFi.FullName);
        //await vault.EliminateExtracted(file);

        private async Task<List<ModifiedUserDataFile>> EliminateAllUnmodifiedFiles(Vault vault)
        {
            var modifiedFiles = new List<ModifiedUserDataFile>();

            foreach (var file in vault.UserDataFiles)
            {
                var encryptedFi = new FileInfo(vault.UserDataPathToEncrypted(file));
                var unlockedFi = new FileInfo(vault.UserDataPathToUnlocked(file));
                
                if (encryptedFi.LastWriteTime == unlockedFi.LastWriteTime)
                {
                    await vault.EliminateExtracted(file);
                }
                else
                {
                    modifiedFiles.Add(new ModifiedUserDataFile(file, unlockedFi.FullName, encryptedFi.FullName));
                }
            }

            return modifiedFiles;
        }

        private async Task UpdateThenEliminateModifiedFiles(Vault vault, List<ModifiedUserDataFile> files)
        {
            await files.ParallelForEachAsync(async modFile =>
            {
                Log.Information("Updating file " + modFile.UnlockedFilePath);
                await vault.WriteDecryptedAsync(modFile.UserDataFile, modFile.UnlockedFilePath,
                    modFile.EncryptedFilePath);

                await vault.EliminateExtracted(modFile.UserDataFile);
            }, 0);
        }
    }
}