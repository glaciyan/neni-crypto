using System.IO;
using NUnit.Framework;

namespace crypto.Core.Tests
{
    [SetUpFixture]
    public class Preparations
    {
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

        public const string TestFolderPath = "../temptestdata/";
        public const string TestDataPath = "../../../testdata/";
    }
}