using System.IO;
using System.Threading.Tasks;

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

        public override async Task Run()
        {
            using var vault = StandardVault.Generate(VaultPath);

            var file = vault.GetFileByPath(TargetPath);

            if (file == null)
            {
                throw new FileNotFoundException("The file to rename wasn't found");
            }

            await vault.RemoveFile(file);
            Notifier.Info("Deleted file " + TargetPath);
        }
    }
}