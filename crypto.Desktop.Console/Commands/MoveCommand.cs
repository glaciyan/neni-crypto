using System.IO;
using System.Threading.Tasks;
using crypto.Desktop.Cnsl.Recources;

namespace crypto.Desktop.Cnsl.Commands
{
    public class MoveCommand : CommandAsync
    {
        public string? VaultPath { get; }
        public string? OldName { get; }
        public string? NewName { get; }

        public MoveCommand(string? vaultPath, string? oldName, string? newName)
        {
            VaultPath = vaultPath ?? throw new NoConsoleArgumentException(Strings.RenameCommand_RenameCommand_No_path_to_vault_given);
            OldName = oldName ?? throw new NoConsoleArgumentException(Strings.RenameCommand_RenameCommand_No_path_to_file_given);
            NewName = newName ?? throw new NoConsoleArgumentException(Strings.MoveCommand_MoveCommand_No_new_path_given);
        }

        public override Task Run()
        {
            using var vault = StandardVault.Generate(VaultPath);

            var file = vault.GetFileByPath(OldName);

            if (file == null)
            {
                throw new FileNotFoundException(Strings.RenameCommand_Run_The_file_to_rename_wasn_t_found);
            }

            vault.MoveFile(file, NewName);
            
            Notifier.Success(Strings.MoveCommand_Run_File_moved_to_ + NewName);
            return Task.CompletedTask;
        }
    }
}