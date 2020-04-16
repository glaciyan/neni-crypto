using System.IO;
using System.Threading.Tasks;
using crypto.Core;

namespace crypto.Desktop.Cnsl.Commands
{
    public class DeleteCommand : CommandAsync
    {
        public string? VaultPath { get; }
        public string? TargetPath { get; }

        public DeleteCommand(string? vaultPath, string? oldName)
        {
            VaultPath = vaultPath ?? throw new NoConsoleArgumentException("No path to vault given");
            TargetPath = oldName ?? throw new NoConsoleArgumentException("No path to file given");
        }

        public override Task Run()
        {
            using var vault = StandardVault.Generate(VaultPath);

            var file = vault.GetFileByPath(TargetPath);

            if (file == null)
            {
                throw new FileNotFoundException("The file to rename wasn't found");
            }

            vault.RemoveFile(file);
            Notifier.Info("Deleted file " + TargetPath);
            
            return Task.CompletedTask;
        }
    }
}