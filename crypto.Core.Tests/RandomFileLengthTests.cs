using System.IO;
using System.Text;
using NUnit.Framework;

namespace crypto.Core.Tests
{
    [TestFixture]
    public class RandomFileLengthTests
    {
        [Test]
        public void Writing_Reading_Works()
        {
            var data = Encoding.ASCII.GetBytes("Random length string");

            var randWriter = new RandomLengthFileContent(data);

            using var memStream = new MemoryStream();
            randWriter.WriteTo(memStream);
            memStream.Position = 0;

            var randReader = new RandomLengthFileContent();

            randReader.ReadFromAsync(memStream);

            Assert.AreEqual(randReader.Content, data);
        }
    }
}