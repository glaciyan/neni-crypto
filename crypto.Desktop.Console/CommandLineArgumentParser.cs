using System;
using crypto.Desktop.Cnsl.Commands;

namespace crypto.Desktop.Cnsl
{
    public static class CommandLineArgumentParser
    {
        public static CommandAsync ParseConfig(string[] args)
        {
            if (args.Length == 0)
                throw new NoConsoleArgumentException("No arguments given");

            var arguments = new ArrayEnumerator<string>(args);

            return arguments.NextOrNull() switch
            {
                // create new project
                "new" => new NewCommandAsync(arguments.NextOrNull(), arguments.NextOrNull()),
                "add" => new AddCommandAsync(arguments.NextOrNull(), arguments.NextOrNull()),
                "unlock" => new UnlockCommandAsync(arguments.NextOrNull()),
                "lock" => new LockCommand(arguments.NextOrNull()),

                _ => throw new ArgumentException("Argument was not recognized")
            };
        }
    }
}