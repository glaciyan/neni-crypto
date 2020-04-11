using System;

namespace crypto.Desktop.Cnsl
{
    public static class Notifier
    {
        public static bool EnableColor { get; set; } = true;

        public static void Error(string message)
        {
            PrintColor(message, ConsoleColor.Red);
        }

        public static void Info(string message)
        {
            PrintColor(message, ConsoleColor.Yellow);
        }

        public static void Success(string message)
        {
            PrintColor(message, ConsoleColor.Green);
        }

        private static void PrintColor(string m, ConsoleColor col)
        {
            if (EnableColor) Console.ForegroundColor = col;

            Console.Error.WriteLine(m);

            if (EnableColor) Console.ResetColor();
        }
    }
}