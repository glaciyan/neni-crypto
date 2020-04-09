using System.Threading.Tasks;

namespace crypto.Desktop.Cnsl.Commands
{
    public abstract class Command
    {
        public abstract Task Run();
    }
}