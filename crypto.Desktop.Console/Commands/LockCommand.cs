using System.Threading.Tasks;
using crypto.Core;
using crypto.Core.Extension;

namespace crypto.Desktop.Cnsl.Commands
{
    public class LockCommand : CommandAsync
    {
        public LockCommand(string? vaultPath)
        {
            VaultPath = vaultPath;
        }

        private string? VaultPath { get; }

        public override Task Run()
        {
            var key = PasswordPrompt.PromptPasswordAsHash();
            
            var paths = new VaultPaths(VaultPath);
            using var vault = Vault.Open(paths, key);
        }
    }
}