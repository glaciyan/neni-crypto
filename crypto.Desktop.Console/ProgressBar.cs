using System;
using System.Text;

namespace crypto.Desktop.Cnsl
{
    public static class ProgressBar
    {
        private static readonly object Locker = new object();
        
        public static void PrintProgressBar(object? sender, ProgressReport e)
        {
            lock (Locker)
            {
                Console.CursorLeft = 0;
                Console.Write($"Progress: {e.ModifiedFiles}/{e.TotalFiles}");
                if (Console.CursorLeft < 30) Console.CursorLeft = 30;
                Console.Write($"{GetBar(e.ModifiedFiles, e.TotalFiles)} Failed: {e.FailedFiles}");
            }
        }

        private static string GetBar(int amount, int outOf)
        {
            var builder = new StringBuilder();
            var completed = (int) Math.Floor((double) amount / outOf * 10);

            builder.Append("[ ");
            for (var i = 0; i < 10; i++)
            {
                if (i <= completed)
                {
                    builder.Append("| ");
                }
                else
                {
                    builder.Append(". ");
                }
            }

            builder.Append("]");

            return builder.ToString();
        }
    }
}