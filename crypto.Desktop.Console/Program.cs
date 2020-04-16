using System;
using System.Threading.Tasks;
using crypto.Desktop.Cnsl.Commands;
using Serilog;

namespace crypto.Desktop.Cnsl
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            // logger setup
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().CreateLogger();

            //parse arguments
            try
            {
                await CommandLineArgumentParser.ParseConfig(args).Run();
            }
            catch (NoConsoleArgumentException)
            {
                Notifier.Info("No console arguments given");
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                Notifier.Error($"Something went wrong: {e.Message}");
            }
        }
    }
}