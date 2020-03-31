using System.IO;
using System.Threading.Tasks;

namespace crypto.Core.File.Format.CryptoConfig
{
    public class VaultFileIOFile : VaultFileIO
    {
        public async Task WriteAsync(string destinationPath, byte[] key)
        {
            await using var writingStream = new FileStream(destinationPath, FileMode.OpenOrCreate);
            await base.WriteAsync(writingStream, key);
        }

        public async Task ReadAsync(string sourcePath, byte[] key)
        {
            await using var readingStream = new FileStream(sourcePath, FileMode.Open);
            await base.ReadAsync(readingStream, key);
        }
    }
}