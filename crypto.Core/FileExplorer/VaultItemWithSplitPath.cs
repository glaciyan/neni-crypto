using System;
using crypto.Core.Header;

namespace crypto.Core.FileExplorer
{
    public class VaultItemWithSplitPath
    {
        public VaultItemWithSplitPath(ItemHeader itemHeader)
        {
            ItemHeader = itemHeader;
            Path = itemHeader.SecuredPlainName.PlainName;
            SplitPath = ItemHeader.SecuredPlainName.PlainName.Replace('\\', '/')
                .Split('/', StringSplitOptions.RemoveEmptyEntries);
        }
        
        
        public ItemHeader ItemHeader { get; }
        public string Path { get; }
        public string[] SplitPath { get; }
    }
}