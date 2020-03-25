using crypto.Core;
using crypto.Core.Cryptography;

namespace crypto.Desktop.Cnsl
{
    // TODO: remove
    public class TestExec : IExecutionConfig
    {
        public TestExec(string source, string destination)
        {
            Source = new CryptoFile(source);
            Destination = destination;
        }

        public CryptoFile Source { get; }
        public string Destination { get; }

        public void Run()
        {
            
        }
    }
}