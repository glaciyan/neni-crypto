﻿using System;
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
                "new" => new NewCommandAsync(arguments.NextOrNull(), arguments.NextOrNull()),
                "add" => new AddCommandAsync(arguments.NextOrNull(), arguments.NextOrNull()),
                "unlock" => new UnlockCommandAsync(arguments.NextOrNull()),
                "lock" => new LockCommand(arguments.NextOrNull()),
                "mv" => new MoveCommand(arguments.NextOrNull(), arguments.NextOrNull(), arguments.NextOrNull()),
                "rn" => new RenameCommand(arguments.NextOrNull(), arguments.NextOrNull(), arguments.NextOrNull()),
                "del" => new DeleteCommand(arguments.NextOrNull(), arguments.NextOrNull()),
                "list" => new ListCommand(arguments.NextOrNull()),
                

                _ => throw new ArgumentException("Argument was not recognized")
            };
        }
    }
}