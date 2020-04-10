using System;
using System.Linq;
using crypto.Core.FileExplorer;
using NUnit.Framework;

namespace crypto.Core.Tests
{
    [TestFixture]
    public class ExplorableTests
    {
        private const string Path1 = "/something/other/pictures/stuff/picture1.png";
        private const string Path2 = "/something/other/secret/encrypted/secretFile.crp";
        private const string Path3 = "/file.txt";

        private readonly Explorer _explorer = new Explorer(Path1, Path2, Path3);
        
        [Test]
        public void BroadPath()
        {
            var files = _explorer.GetFromPath("/something/other/").ToList();
            Assert.IsTrue(files.Count == 2);

            var (_, fileFolder, index) = files[0];
            Assert.AreEqual(FileFolder.Folder, fileFolder);
            Assert.AreEqual(2, index);

            (_, fileFolder, index) = files[1];
            Assert.AreEqual(FileFolder.Folder, fileFolder);
            Assert.AreEqual(2, index);
        }

        [Test]
        public void Tiny()
        {
            FileFolder fileFolder;
            int index;
            var files = _explorer.GetFromPath("/something/other/secret").ToList();
            Assert.IsTrue(files.Count == 1);

            (_, fileFolder, index) = files[0];
            Assert.AreEqual(FileFolder.Folder, fileFolder);
            Assert.AreEqual(3, index);
        }

        [Test]
        public void File()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _explorer.GetFromPath("/something/other/secret/encrypted/secretFile.crp");
            });
        }

        [Test]
        public void Root()
        {
            var files = _explorer.GetFromPath("").ToList();
            Assert.IsTrue(files.Count == 3);

            var (_, fileFolder, index) = files[2];
            Assert.AreEqual(FileFolder.File, fileFolder);
            Assert.AreEqual(0, index);
        }
    }
}