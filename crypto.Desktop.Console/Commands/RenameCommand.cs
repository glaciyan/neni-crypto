using System.IO;
using System.Threading.Tasks;
using crypto.Core;

namespace crypto.Desktop.Cnsl.Commands
{
    public class RenameCommand : CommandAsync
    {
        public string? VaultPath { get; }
        public string? OldName { get; }
        public string? NewName { get; }

        public RenameCommand(string? vaultPath, string? oldName, string? newName)
        {
            VaultPath = vaultPath ?? throw new NoConsoleArgumentException("No path to vault given");
            OldName = oldName ?? throw new NoConsoleArgumentException("No path to file given");
            NewName = newName ?? throw new NoConsoleArgumentException("No new name given");
        }
        
        public override Task Run()
        {
            using var vault = StandardVault.Generate(VaultPath);

            var file = vault.GetFileByPath(OldName);

            if (file == null)
            {
                throw new FileNotFoundException("The file to rename wasn't found");
            }

            vault.RenameFile(file, NewName);
            
            Notifier.Success("File renamed to " + NewName);
            return Task.CompletedTask;
        }
    }
}