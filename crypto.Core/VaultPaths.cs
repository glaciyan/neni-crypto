using System.IO;
using crypto.Core.Recources;

namespace crypto.Core
{
    public class VaultPaths
    {
        public VaultPaths(string folderPath)
        {
            FullVaultFolderPath = Path.GetFullPath(folderPath);
            var fullPathDirInfo = new DirectoryInfo(FullVaultFolderPath);

            Name = fullPathDirInfo.Name;

            VaultFilePath = Vault.GetVaultFilePath(FullVaultFolderPath, Name);

            if (!File.Exists(VaultFilePath))
                throw new FileNotFoundException(string.Format(Strings.VaultPaths_VaultPaths_Couldn_t_find_vault_file_for_path__0_, VaultFilePath));
        }

        public string Name { get; set; }
        public string FullVaultFolderPath { get; set; }
        public string VaultFilePath { get; set; }
    }
}