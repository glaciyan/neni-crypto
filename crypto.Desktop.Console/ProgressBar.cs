using System;
using System.Text;
using crypto.Desktop.Cnsl.Recources;

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
                Console.Write(Strings.ProgressBar_PrintProgressBar_Progress___0___1_, e.ModifiedFiles, e.TotalFiles);
                if (Console.CursorLeft < 30) Console.CursorLeft = 30;
                Console.Write(Strings.ProgressBar_PrintProgressBar__0__Failed___1_, GetBar(e.ModifiedFiles, e.TotalFiles), e.FailedFiles);
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