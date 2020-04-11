namespace crypto.Core.FileExplorer
{
    public class ExplorableVaultItemTypeIndex
    {
        public VaultItemWithSplitPath VaultItemWithSplitPath;
        public FileFolder Type;
        public int Index;

        public ExplorableVaultItemTypeIndex(VaultItemWithSplitPath vaultItemWithSplitPath, FileFolder type, int index)
        {
            VaultItemWithSplitPath = vaultItemWithSplitPath;
            Type = type;
            Index = index;
        }
    }
}