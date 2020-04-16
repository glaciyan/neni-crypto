using System;
using System.Runtime.CompilerServices;

namespace crypto.Desktop.Cnsl
{
    public static class FilesProgressBar
    {
        private static readonly object Locker = new object();
        
        public static void PrintProgressBar(int done, int outOf, int failed)
        {
            lock (Locker)
            {
                Console.CursorLeft = 0;
                Console.Write($"Progress: {done}/{outOf} Failed: {failed}");
            }
        }
    }
}