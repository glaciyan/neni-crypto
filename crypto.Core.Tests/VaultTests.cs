using System.IO;
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
        private const string TestFolderPath = "../temptestdata/";
        private const string TestDataPath = "../../../../testdata/";

        [OneTimeSetUp]
        public void Set_Up()
        {
            Directory.CreateDirectory(TestFolderPath);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Directory.Delete(TestFolderPath, true);
        }

        private byte[] GetFileHash(string path)
        {
            using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var sha256 = SHA256.Create();

            return sha256.ComputeHash(fileStream);
        }

        [Test]
        public void CreateCryptoConfigNoPrefixPath()
        {
            const string targetFile = TestFolderPath + "CreateCryptoConfigNoPrefixPath.td";
            const string mockFileName = "importantData.txt";

            var key = CryptoRNG.GetRandomBytes(AesSizes.Key);

            var vaultFile = ItemHeader.Create(mockFileName);
            var writer = new ItemHeaderWriter(vaultFile);

            using (var stream = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
            {
                writer.WriteTo(stream, key);
            }

            ItemHeader readItemHeader;

            using (var stream = new FileStream(targetFile, FileMode.Open, FileAccess.Read))
            {
                readItemHeader = ItemHeaderReader.ReadFrom(stream, key);
            }

            Assert.AreEqual(vaultFile.TargetPath, readItemHeader.TargetPath);
            Assert.AreEqual(vaultFile.TargetCipherIV, readItemHeader.TargetCipherIV);
            Assert.AreEqual(vaultFile.TargetAuthentication, readItemHeader.TargetAuthentication);

            Assert.AreEqual(vaultFile.IsUnlocked, readItemHeader.IsUnlocked);
            Assert.AreEqual(vaultFile.SecuredPlainName.PlainName, readItemHeader.SecuredPlainName.PlainName);
        }

        [Test]
        public async Task DecryptingFile()
        {
            const string vaultName = "DFile";
            const string testFile = TestDataPath + "DecryptingFile.dat";
            var unlockedPath = $"{TestFolderPath}{vaultName}/Unlocked/DecryptingFile.dat";
            var key = CryptoRNG.GetRandomBytes(AesSizes.Key);

            var originalHash = GetFileHash(testFile);

            // create an encrypted file
            {
                var vault = Vault.Create(vaultName, TestFolderPath);
                await vault.AddFileAsync(testFile);

                VaultReaderWriter.WriteConfig(vault, key);
            }

            // decrypt the file
            {
                var vault = VaultReaderWriter.ReadFromConfig($"{TestFolderPath}{vaultName}", key);
                var hashMatches = await vault.ExtractFile(vault.ItemHeaders[0]);
                Assert.IsTrue(hashMatches);

                Assert.IsTrue(vault.ItemHeaders[0].IsUnlocked);

                await vault.EliminateExtracted(vault.ItemHeaders[0]);
                Assert.IsFalse(vault.ItemHeaders[0].IsUnlocked);
                
                await vault.ExtractFile(vault.ItemHeaders[0]);
                Assert.IsTrue(vault.ItemHeaders[0].IsUnlocked);
            }

            var decryptedHash = GetFileHash(unlockedPath);

            Assert.AreEqual(originalHash, decryptedHash);
        }

        [Test]
        public async Task VaultItemHeadersFileWriteRead()
        {
            const string vaultName = "TestVault";
            const string testFile = TestDataPath + "data.dat";
            var key = "passphrase".ApplySHA256();

            var file = Vault.Create(vaultName, TestFolderPath);
            await file.AddFileAsync(testFile);
            VaultReaderWriter.WriteConfig(file, key);

            var readFile = VaultReaderWriter.ReadFromConfig($"{TestFolderPath}/{vaultName}", key);
            await readFile.AddFileAsync(testFile, "others");

            var readFile2 = VaultReaderWriter.ReadFromConfig($"{TestFolderPath}/{vaultName}", key);
        }

        [Test]
        public void WriterReaderVaultHeader()
        {
            const string targetPath = TestFolderPath + "WriterReaderVaultHeader.td";
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