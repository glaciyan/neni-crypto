namespace crypto.Core.FileExplorer
{
    public class ExplorableVaultItem
    {
        public VaultItemWithSplitPath VaultItemWithSplitPath;
        public FileFolder Type;
        public int Index;

        public ExplorableVaultItem(VaultItemWithSplitPath vaultItemWithSplitPath, FileFolder type, int index)
        {
            VaultItemWithSplitPath = vaultItemWithSplitPath;
            Type = type;
            Index = index;
        }
    }
}