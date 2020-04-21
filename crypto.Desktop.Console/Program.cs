using System;
using System.Threading.Tasks;
using crypto.Desktop.Cnsl.Commands;
using crypto.Desktop.Cnsl.Recources;
using Serilog;

namespace crypto.Desktop.Cnsl
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            // logger setup
            #if DEBUG
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().CreateLogger();
            #endif
            
            try
            {
                await CommandLineArgumentParser.ParseConfig(args).Run();
            }
            catch (NoConsoleArgumentException)
            {
                Notifier.Info(Strings.Program_Main_No_console_arguments_given);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                Notifier.Error(Strings.Program_Main_Something_went_wrong__ + e.Message);
            }
        }
    }
}