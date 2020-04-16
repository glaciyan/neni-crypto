using System.IO;
using System.Threading.Tasks;
using crypto.Core;

namespace crypto.Desktop.Cnsl.Commands
{
    public class MoveCommand : CommandAsync
    {
        public string? VaultPath { get; }
        public string? OldName { get; }
        public string? NewName { get; }

        public MoveCommand(string? vaultPath, string? oldName, string? newName)
        {
            VaultPath = vaultPath ?? throw new NoConsoleArgumentException("No path to vault given");
            OldName = oldName ?? throw new NoConsoleArgumentException("No path to file given");
            NewName = newName ?? throw new NoConsoleArgumentException("No new name given");
        }

        public override Task Run()
        {
            var key = PasswordPrompt.PromptPasswordAsHash();

            var paths = new VaultPaths(VaultPath);
            using var vault = Vault.Open(paths, key);

            var file = vault.GetFileByPath(OldName);

            if (file == null)
            {
                throw new FileNotFoundException("The file to rename wasn't found");
            }

            vault.MoveFile(file, NewName);
            
            return Task.CompletedTask;
        }
    }
}