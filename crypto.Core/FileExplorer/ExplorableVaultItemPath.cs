using System;
using crypto.Core.Header;

namespace crypto.Core.FileExplorer
{
    public class ExplorableVaultItemPath
    {
        public ExplorableVaultItemPath(ItemHeader itemHeader)
        {
            ItemHeader = itemHeader;
            SplitPath = ItemHeader.SecuredPlainName.PlainName.Replace('\\', '/')
                .Split('/', StringSplitOptions.RemoveEmptyEntries);
        }

        // string[], FileFolder, int
        public ItemHeader ItemHeader { get; }
        public string[] SplitPath { get; }
    }
}