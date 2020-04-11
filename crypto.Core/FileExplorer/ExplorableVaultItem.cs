namespace crypto.Core.FileExplorer
{
    public class ExplorableVaultItem
    {
        public int Index;
        public FileFolder Type;
        public VaultItemWithSplitPath VaultItemWithSplitPath;

        public ExplorableVaultItem(VaultItemWithSplitPath vaultItemWithSplitPath, FileFolder type, int index)
        {
            VaultItemWithSplitPath = vaultItemWithSplitPath;
            Type = type;
            Index = index;
        }
    }
}