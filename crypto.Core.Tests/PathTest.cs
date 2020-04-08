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
            const string expected = "other/more/stuff/test/mock";

            var result = NDirectory.GetPathToFile(testPath);
            
            Assert.AreEqual(expected, result);
        }
    }
}