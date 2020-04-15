using System;
using crypto.Core.Header;

namespace crypto.Core.FileExplorer
{
    public class VaultItemWithSplitPath
    {
        public VaultItemWithSplitPath(UserDataHeader userDataHeader)
        {
            UserDataHeader = userDataHeader;
            Path = userDataHeader.SecuredPlainName.PlainName;
            SplitPath = UserDataHeader.SecuredPlainName.PlainName.Replace('\\', '/')
                .Split('/', StringSplitOptions.RemoveEmptyEntries);
        }


        public UserDataHeader UserDataHeader { get; }
        public string Path { get; }
        public string[] SplitPath { get; }
    }
}