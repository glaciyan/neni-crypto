using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using crypto.Core.Cryptography;
using crypto.Core.Extension;
using crypto.Core.Header;
using NUnit.Framework;

namespace crypto.Core.Tests
{
    [TestFixture]
    public class VaultTests
    {
        private byte[] GetFileHash(string path)
        {
            using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var sha256 = SHA256.Create();

            return sha256.ComputeHash(fileStream);
        }

        [Test]
        public void CreateCryptoConfigNoPrefixPath()
        {
            const string targetFile = Preparations.TestFolderPath + "CreateCryptoConfigNoPrefixPath.td";
            const string mockFileName = "importantData.txt";

            var key = CryptoRNG.GetRandomBytes(AesSizes.Key);

            var vaultFile = UserDataHeader.Create(mockFileName);
            var writer = new UserDataHeaderWriter(vaultFile);

            using (var stream = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
            {
                writer.WriteTo(stream, key);
            }

            UserDataHeader readUserDataHeader;

            using (var stream = new FileStream(targetFile, FileMode.Open, FileAccess.Read))
            {
                readUserDataHeader = UserDataHeaderReader.ReadFrom(stream, key);
            }

            Assert.AreEqual(vaultFile.TargetPath, readUserDataHeader.TargetPath);
            Assert.AreEqual(vaultFile.TargetCipherIV, readUserDataHeader.TargetCipherIV);
            Assert.AreEqual(vaultFile.TargetAuthentication, readUserDataHeader.TargetAuthentication);

            Assert.AreEqual(vaultFile.IsUnlocked, readUserDataHeader.IsUnlocked);
            Assert.AreEqual(vaultFile.SecuredPlainName.PlainName, readUserDataHeader.SecuredPlainName.PlainName);
        }

        [Test]
        [Order(0)]
        public async Task DecryptingFile()
        {
            const string vaultName = "DFile";
            const string testFile = Preparations.TestDataPath + "DecryptingFile.dat";
            var unlockedPath = $"{Preparations.TestFolderPath}{vaultName}/Unlocked/DecryptingFile.dat";
            var key = CryptoRNG.GetRandomBytes(AesSizes.Key);

            var originalHash = GetFileHash(testFile);

            // create an encrypted file
            {
                using var vault = Vault.Create(vaultName, key, Preparations.TestFolderPath);
                await vault.AddFileAsync(testFile);
            }

            // decrypt the file
            {
                var paths = new VaultPaths($"{Preparations.TestFolderPath}{vaultName}/");
                using var vault = VaultReaderWriter.ReadFromConfig(paths, key);
                var status = await vault.ExtractFile(vault.UserDataFiles.First());
                Assert.AreEqual(ExtractStatus.Ok, status);

                Assert.IsTrue(vault.UserDataFiles.First().Header.IsUnlocked);

                await vault.EliminateExtracted(vault.UserDataFiles.First());
                Assert.IsFalse(vault.UserDataFiles.First().Header.IsUnlocked);

                await vault.ExtractFile(vault.UserDataFiles.First());
                Assert.IsTrue(vault.UserDataFiles.First().Header.IsUnlocked);
            }

            var decryptedHash = GetFileHash(unlockedPath);

            Assert.AreEqual(originalHash, decryptedHash);
        }

        [Test]
        [Order(2)]
        public async Task MoveFileInVault()
        {
            const string vaultName = "MoveFileInVault";
            const string testFile = Preparations.TestDataPath + "DecryptingFile.dat";
            var key = CryptoRNG.GetRandomBytes(AesSizes.Key);

            using var vault = Vault.Create(vaultName, key, Preparations.TestFolderPath);
            await vault.AddFileAsync(testFile);

            await vault.ExtractFile(vault.UserDataFiles.First());

            vault.MoveFile(vault.UserDataFiles.First(), "other/files/DecryptingFile.dat");
            vault.RenameFile(vault.UserDataFiles.First(), "File.dat");

            Assert.IsTrue(File.Exists($"{Preparations.TestFolderPath}{vaultName}/Unlocked/other/files/File.dat"));
            Assert.AreEqual(vault.UserDataFiles.First().Header.SecuredPlainName.PlainName, "other/files/File.dat");
        }

        [Test]
        [Order(1)]
        public async Task RemoveFileFromVault()
        {
            const string vaultName = "RemoveFileFromVault";
            const string testFile = Preparations.TestDataPath + "DecryptingFile.dat";
            var key = CryptoRNG.GetRandomBytes(AesSizes.Key);

            using var vault = Vault.Create(vaultName, key, Preparations.TestFolderPath);
            await vault.AddFileAsync(testFile);

            await vault.RemoveFile(vault.UserDataFiles.First());
        }

        [Test]
        public async Task VaultItemHeadersFileWriteRead()
        {
            const string vaultName = "TestVault";
            const string testFile = Preparations.TestDataPath + "data.dat";
            var key = "passphrase".ApplySHA256();

            using var file = Vault.Create(vaultName, key, Preparations.TestFolderPath);
            await file.AddFileAsync(testFile);
            VaultReaderWriter.WriteConfig(file, key);

            var paths = new VaultPaths($"{Preparations.TestFolderPath}/{vaultName}/");
            var readFile = VaultReaderWriter.ReadFromConfig(paths, key);
            await readFile.AddFileAsync(testFile, "others");

            var unused = VaultReaderWriter.ReadFromConfig(paths, key);
        }

        [Test]
        public void WriterReaderVaultHeader()
        {
            const string targetPath = Preparations.TestFolderPath + "WriterReaderVaultHeader.td";
            var key = CryptoRNG.GetRandomBytes(AesSizes.Key);

            var header = VaultHeader.Create();

            using (var stream = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
            {
                var writer = new VaultHeaderWriter(header);
                writer.WriteTo(stream, key);
            }

            VaultHeader readHeader;
            using (var stream = new FileStream(targetPath, FileMode.Open, FileAccess.Read))
            {
                readHeader = VaultHeaderReader.ReadFrom(stream);
            }

            Assert.IsTrue(readHeader.MasterPassword.GetDecryptedPassword(key).Item1);
        }
    }
}