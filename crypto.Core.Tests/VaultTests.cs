using System;
using System.IO;
using System.Threading.Tasks;
using crypto.Core.Cryptography;
using crypto.Core.Header;
using NUnit.Framework;

namespace crypto.Core.Tests
{
    [TestFixture]
    public class VaultTests
    {
        private const string FolderPath = "TestVault";
        private const string TestFolderPath = "../testData/";
        private const string FilePath = "../test/mock/testFile.txt";
        private const string TestFile = "../../../../testdata/data.dat";

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

        [Test]
        public async Task VaultItemHeadersFileWriteRead()
        {
            var key = "passphrase".ApplySHA256();
            
            using (var file = VaultItemHeadersFile.Create(FolderPath, TestFolderPath, key))
                await file.AddFileAsync(TestFile);
            
            key = "passphrase".ApplySHA256();
            
            using (var readFile = VaultItemHeadersFile.ReadFrom($"{TestFolderPath}/{FolderPath}", key))
                await readFile.AddFileAsync(TestFile);
            
            key = "passphrase".ApplySHA256();

            var readFile2 = VaultItemHeadersFile.ReadFrom($"{TestFolderPath}/{FolderPath}", key);
        }
    }
}