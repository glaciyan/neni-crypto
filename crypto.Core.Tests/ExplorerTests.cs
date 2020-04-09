using crypto.Core.FileExplorer;
using NUnit.Framework;

namespace crypto.Core.Tests
{
    [TestFixture]
    public class ExplorerTests
    {
        [Test]
        public void AddingItemToFolder()
        {
            var dir = new ExplorerDirectory("Test");
            var dir2 = new ExplorerDirectory("folder1");
            var dir3 = new ExplorerDirectory("folder1");
            var file1 = new ExplorerFile("testFile");
            var file2 = new ExplorerFile("folder1");
            
            dir.AddChild(dir2);
            
            dir3.AddChild(file1);
            dir.AddChild(dir3);
            dir.AddChild(file2);
        }
    }
}