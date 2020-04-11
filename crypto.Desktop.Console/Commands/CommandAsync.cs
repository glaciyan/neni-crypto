using System.Threading.Tasks;

namespace crypto.Desktop.Cnsl.Commands
{
    public abstract class CommandAsync
    {
        public abstract Task Run();
    }
}