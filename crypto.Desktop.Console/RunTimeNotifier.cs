using System;

namespace crypto.Desktop.Cnsl
{
    public static class RunTimeNotifier
    {
        public static void Error(string message, bool color = true)
        {
            if (color) Console.ForegroundColor = ConsoleColor.Red;

            Console.Error.WriteLine(message);

            if (color) Console.ResetColor();
        }
    }
}