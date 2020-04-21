using System;
using System.IO;
using System.Threading.Tasks;
using crypto.Core;
using crypto.Desktop.Cnsl.Recources;
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
            using var vault = StandardVault.Generate(VaultPath);

            Notifier.Info(Strings.LockCommand_Run_Locking_files___);
            await LockAllFiles(vault);
            
            Notifier.Success(Strings.LockCommand_Run_Locked_all_files_in_Vault_);
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
                        Log.Information(Strings.LockCommand_LockAllFiles_Updating_file_ + modFile.UnlockedFilePath);
                        await vault.WriteDecryptedAsync(modFile.UserDataFile, modFile.UnlockedFilePath,
                            modFile.EncryptedFilePath);

                        await vault.EliminateExtracted(modFile.UserDataFile);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(string.Format(Strings.LockCommand_LockAllFiles_Error_locking_file__0____1_, file.Header.SecuredPlainName.PlainName, e));
                }
            }, 0);
        }
    }
}