using System.IO;
using System.Threading.Tasks;

namespace crypto.Core.File.Format.CryptoConfig
{
    public class VaultFileIO
    {
        private readonly VaultFileFormat _format;

        public VaultFileIO(VaultFile file)
        {
            _format = new VaultFileFormat(file);
        }

        public async Task WriteAsync(Stream destination, byte[] key)
        {
            throw new System.NotImplementedException();
        }

        public async Task ReadAsync(Stream source, byte[] key)
        {
            throw new System.NotImplementedException();
        }
    }
}