using System.IO;
using System.Threading.Tasks;

namespace crypto.Core.File.Format.Vault
{
    public class VaultIO
    {
        private readonly VaultFormat _format;

        public VaultIO(File.Vault vaultFileConfig)
        {
            _format = new VaultFormat(vaultFileConfig);
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