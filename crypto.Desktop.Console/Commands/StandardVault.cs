using crypto.Core;

namespace crypto.Desktop.Cnsl.Commands
{
    public static class StandardVault
    {
        public static Vault Generate(string? vaultPath)
        {
            var key = PasswordPrompt.PromptPasswordAsHash();

            var paths = new VaultPaths(vaultPath);
            return Vault.Open(paths, key);
        }
    }
}