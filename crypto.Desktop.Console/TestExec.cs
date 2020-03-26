using crypto.Core;

namespace crypto.Desktop.Cnsl
{
    // TODO: remove
    public class TestExec : IExecutionConfig
    {
        public TestExec(string source, string destination)
        {
            Source = new CipherFile(source);
            Destination = destination;
        }

        public CipherFile Source { get; }
        public string Destination { get; }

        public void Run()
        {
            
        }
    }
}