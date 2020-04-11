using System;
using System.Linq;
using crypto.Core.FileExplorer;
using crypto.Core.Header;
using NUnit.Framework;

namespace crypto.Core.Tests
{
    [TestFixture]
    public class ExplorableTests
    {
        private readonly ItemHeader _path1 = ItemHeader.Create("picture1.png", "/something/other/pictures/stuff/");
        private readonly ItemHeader _path2 = ItemHeader.Create("secretFile.crp", "/something/other/secret/encrypted/");
        private readonly ItemHeader _path3 = ItemHeader.Create("/file.txt");

        private Explorer _explorer;
        
        [OneTimeSetUp]
        public void SetUpExplorer()
        {
            _explorer = new Explorer(_path1, _path2, _path3);
        }
        
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