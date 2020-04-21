using System.IO;
using System.Threading.Tasks;
using crypto.Desktop.Cnsl.Recources;

namespace crypto.Desktop.Cnsl.Commands
{
    public class DeleteCommand : CommandAsync
    {
        public string? VaultPath { get; }
        public string? TargetPath { get; }

        public DeleteCommand(string? vaultPath, string? oldName)
        {
            VaultPath = vaultPath ?? throw new NoConsoleArgumentException(Strings.RenameCommand_RenameCommand_No_path_to_vault_given);
            TargetPath = oldName ?? throw new NoConsoleArgumentException(Strings.RenameCommand_RenameCommand_No_path_to_file_given);
        }

        public override async Task Run()
        {
            using var vault = StandardVault.Generate(VaultPath);

            var file = vault.GetFileByPath(TargetPath);

            if (file == null)
            {
                throw new FileNotFoundException(Strings.RenameCommand_Run_The_file_to_rename_wasn_t_found);
            }

            await vault.RemoveFile(file);
            Notifier.Info(string.Format(Strings.DeleteCommand_Run_Deleted_file_, TargetPath));
        }
    }
}