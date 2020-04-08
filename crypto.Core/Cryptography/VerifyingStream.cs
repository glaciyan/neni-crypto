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
            var hash = new byte[32];

            var buffer = ArrayPool<byte>.Shared.Rent(BufferSize);

            try
            {
                int readBytes;
                while ((readBytes = await source.ReadAsync(buffer)) != 0)
                {
                    await destination.WriteAsync(buffer, 0, readBytes);

                    if (readBytes >= BufferSize)
                        sha.TransformBlock(buffer, 0, buffer.Length, hash, 0);
                    else
                        sha.TransformFinalBlock(buffer, 0, readBytes);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }

            return sha.Hash;
        }
    }
}