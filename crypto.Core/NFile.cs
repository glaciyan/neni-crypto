using System.Buffers;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using crypto.Core.Cryptography;
using crypto.Core.Extension;

namespace crypto.Core
{
    public static class NFile
    {
        public static async Task Purge(string filePath)
        {
            await using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Write);

            var zeroBuffer = ArrayPool<byte>.Shared.Rent(VerifyingStream.BufferSize);
            zeroBuffer.Zeros();

            try
            {
                while (fs.Position < fs.Length) await fs.WriteAsync(zeroBuffer);
                await fs.DisposeAsync();

                File.Delete(filePath);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(zeroBuffer);
            }
        }

        public static byte[] GetHash(string path)
        {
            using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var sha256 = SHA256.Create();

            return sha256.ComputeHash(fileStream);
        }
    }
}