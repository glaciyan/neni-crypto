using NUnit.Framework;

namespace crypto.Core.Tests
{
    [TestFixture]
    public class PathTest
    {
        [Test]
        public void GetPathToFileGivesCorrectPath()
        {
            const string testPath = "other/more/stuff/test/mock/file.txt";
            const string expected = "other/more/stuff/test/mock/";

            var result = NDirectory.GetPathParentDir(testPath);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RemoveRelativeParts()
        {
            const string testPath = "../test/./other/stuff/file.txt";
            const string expected = "test/other/stuff/file.txt";

            var result = NPath.RemoveRelativeParts(testPath);
            
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RelativePathToFileTest()
        {
            const string path = "other/more/stuff/test/mock/file.txt";
            const string relativeTo = "other/more/stuff/";

            var resultPath = NPath.GetRelativePathToFile(relativeTo, path);
            const string expected = "stuff/test/mock/";
            
            Assert.AreEqual(expected, resultPath);
        }
    }
}