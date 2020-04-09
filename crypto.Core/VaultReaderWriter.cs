using System.IO;
using System.Security.Cryptography;
using crypto.Core.Header;

namespace crypto.Core
{
    public static class VaultReaderWriter
    {
        //++++++++++++++++++++++++++++
        //        Reading
        //++++++++++++++++++++++++++++

        public static Vault ReadFromConfig(string folderPath, byte[] key)
        {
            var (fullFolderPath, folderName, vaultFilePath) = VerifyPathAndGetNames(folderPath);
            var result = new Vault(folderName, key) {VaultPath = fullFolderPath};

            using var vaultFile = new FileStream(vaultFilePath, FileMode.Open, FileAccess.Read);

            result.Header = VaultHeaderReader.ReadFrom(vaultFile);

            var (keyWasCorrect, password) = result.Header.MasterPassword.GetDecryptedPassword(key);

            if (!keyWasCorrect) throw new CryptographicException("Password wasn't able to be verified");

            while (vaultFile.Position < vaultFile.Length)
                result.ItemHeaders.Add(ItemHeaderReader.ReadFrom(vaultFile, password));

            // TODO: if a file is unlocked check if the unlocked file exists in method from vault
            
            return result;
        }

        private static (string, string, string) VerifyPathAndGetNames(string path)
        {
            var fullPath = Path.GetFullPath(path);
            var folderName = Path.GetFileNameWithoutExtension(fullPath);
            var vaultFilePath = Vault.GetVaultFilePath(fullPath, folderName);

            if (File.Exists(vaultFilePath)) return (fullPath, folderName, vaultFilePath);

            throw new FileNotFoundException("Couldn't find vault file for path" + path);
        }

        //++++++++++++++++++++++++++++
        //        Writing
        //++++++++++++++++++++++++++++

        public static void WriteConfig(Vault underlying, byte[] key)
        {
            using var fileStream = new FileStream(underlying.VaultFilePath, FileMode.Open);

            WriteHeader(fileStream, underlying, key);

            using var binWriter = new BinaryWriter(fileStream);

            foreach (var itemHeader in underlying.ItemHeaders) WriteItemHeader(fileStream, underlying, itemHeader);

            underlying.Written = true;
        }

        private static void WriteHeader(Stream fileStream, Vault underlying, byte[] key)
        {
            var headerWriter = new VaultHeaderWriter(underlying.Header);
            headerWriter.WriteTo(fileStream, key);
        }

        private static void WriteItemHeader(Stream fileStream, Vault underlying, ItemHeader itemHeader)
        {
            var itemHeaderWriter = new ItemHeaderWriter(itemHeader);
            itemHeaderWriter.WriteTo(fileStream, underlying.Header.MasterPassword.Password);
        }
    }
}