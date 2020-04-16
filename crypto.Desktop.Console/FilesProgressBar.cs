using System;

namespace crypto.Desktop.Cnsl
{
    public static class FilesProgressBar
    {
        private static readonly object Locker = new object();
        
        public static void PrintProgressBar(object? sender, ProgressReport e)
        {
            lock (Locker)
            {
                Console.CursorLeft = 0;
                Console.Write($"Progress: {e.ModifiedFiles}/{e.TotalFiles} Failed: {e.FailedFiles}");
            }
        }
    }
}