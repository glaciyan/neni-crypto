using System.IO;

namespace crypto.Core
{
    public static class NDirectory
    {
        public static void CleanEmptyDirectories(string root)
        {
            var dirInfo = new DirectoryInfo(root);

            if (dirInfo.GetDirectories().Length == 0)
            {
                dirInfo.Delete();
                return;
            }
            
            RecursiveCleanUp(dirInfo);
        }

        private static void RecursiveCleanUp(DirectoryInfo dirInfo)
        {
            foreach (var directoryInfo in dirInfo.EnumerateDirectories())
            {
                var dirs = directoryInfo.GetDirectories();

                if (dirs.Length == 0) directoryInfo.Delete();
                else
                {
                    RecursiveCleanUp(directoryInfo);
                }
            }
        }
    }
}