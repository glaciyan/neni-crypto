using System.Threading.Tasks;
using crypto.Core;
using crypto.Core.Extension;

namespace crypto.Desktop.Cnsl.Commands
{
    public class OpenCommand : Command
    {
        private string? VaultPath { get; }

        public OpenCommand(string? vaultPath)
        {
            VaultPath = vaultPath;
        }

        public override async Task Run()
        {
            await Task.Run(() =>
            {
                var key = PasswordPrompt.PromptPassword().ApplySHA256();
                
                var vault = Vault.Open(VaultPath, key);
                
                var browser = new VaultBrowser(vault);
                
                browser.Display("crypto.Core/Cryptography/");
            });
        }
    }
}