using System.Buffers;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace crypto.Core.Cryptography
{
    public static class VerifyingStream
    {
        // default size in c# docs about Stream.CopyTo(Stream, Int32)
        public const int BufferSize = 81920;

        public static async Task<byte[]> CopyToCreateHashAsync(this Stream source, Stream destination)
        {
            using var sha = SHA256.Create();

            var buffer = ArrayPool<byte>.Shared.Rent(BufferSize);
            var currentBufferLength = buffer.Length;

            try
            {
                var lastBufferSize = 0;

                int readBytes;
                while ((readBytes = await source.ReadAsync(buffer)) != 0)
                {
                    await destination.WriteAsync(buffer, 0, readBytes);

                    if (readBytes == currentBufferLength)
                        sha.TransformBlock(buffer, 0, buffer.Length, buffer, 0);
                    else
                        lastBufferSize = readBytes;
                }

                sha.TransformFinalBlock(buffer, 0, lastBufferSize);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }

            return sha.Hash;
        }
    }
}