using crypto.Core.FileExplorer;
using NUnit.Framework;

namespace crypto.Core.Tests
{
    [TestFixture]
    public class ExplorableTests
    {
        [Test]
        public void GettingCorrectCollection()
        {
            var path1 = "/something/other/pictures/stuff/picture1.png";
            var path2 = "/something/other/secret/encrypted/secretFile.crp";

            var pathToExplore = "/something/other/";
            var pathToExplore2 = "/something/other/secret";
            var pathToExplore3 = "/something/other/secret/encrypted/secretFile.crp";


            var explorer = new Explorer(path1, path2);

            var files1 = explorer.GetFromPath(pathToExplore);
            var files2 = explorer.GetFromPath(pathToExplore2);
            var files3 = explorer.GetFromPath(pathToExplore3);

            
        }
    }
}