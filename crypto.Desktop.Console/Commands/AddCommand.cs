using System;
using System.Threading.Tasks;
using crypto.Core;
using crypto.Core.Extension;
using Serilog;

namespace crypto.Desktop.Cnsl.Commands
{
    public class AddCommand : Command
    {
        private string? ToAddPath { get; }
        private string? VaultPath { get; }

        /* add -> error missing file
         * add fileName -> look is current dir is a vault then add that file if true
         * add fileName vaultPath -> look if the given dir is a vault then add it if true
         */
        
        public AddCommand(string? addPath, string? vaultPath)
        {
            ToAddPath = addPath ?? throw new NullReferenceException("Path to file not given");
            VaultPath = vaultPath;
        }

        public override async Task Run()
        {
            var key = PasswordPrompt.PromptPassword().ApplySHA256();
            using var vault = VaultPath == null ? Vault.Open(Environment.CurrentDirectory, key) : Vault.Open(VaultPath, key);

            await vault.AddFileAsync(ToAddPath);
            
            Log.Debug("Added file to vault");
        }
    }
}