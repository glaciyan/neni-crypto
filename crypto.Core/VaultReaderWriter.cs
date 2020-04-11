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

        public static Vault ReadFromConfig(VaultReadingPaths readingPaths, byte[] key)
        {
            var result = new Vault(readingPaths.Name, key) {VaultPath = readingPaths.FullVaultFolderPath};

            using var vaultFile = new FileStream(readingPaths.VaultFilePath, FileMode.Open, FileAccess.Read);

            result.Header = VaultHeaderReader.ReadFrom(vaultFile);

            var (keyWasCorrect, password) = result.Header.MasterPassword.GetDecryptedPassword(key);

            if (!keyWasCorrect) throw new CryptographicException("Password wasn't able to be verified");

            while (vaultFile.Position < vaultFile.Length)
                result.ItemHeaders.Add(ItemHeaderReader.ReadFrom(vaultFile, password));

            result.CheckAndCorrectAllItemHeaders();

            return result;
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