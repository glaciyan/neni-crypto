using System;
using System.Threading.Tasks;

namespace crypto.Desktop.Cnsl.Commands
{
    public class ListCommand : CommandAsync
    {
        public string? VaultPath { get; set; }

        public ListCommand(string? vaultPath)
        {
            VaultPath = vaultPath ?? Environment.CurrentDirectory;
        }


        public override Task Run()
        {
            using var vault = StandardVault.Generate(VaultPath);

            foreach (var file in vault.UserDataFiles)
            {
                Console.WriteLine(file.Header.SecuredPlainName.PlainName);
            }
            
            return Task.CompletedTask;
        }
    }
}