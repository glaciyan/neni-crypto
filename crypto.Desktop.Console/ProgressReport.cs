using System.Threading;

namespace crypto.Desktop.Cnsl
{
    public class ProgressReport
    {
        private int _modifiedFiles;
        private int _failedFiles;

        public int ModifiedFiles
        {
            get => _modifiedFiles;
            set => _modifiedFiles = value;
        }

        public int FailedFiles
        {
            get => _failedFiles;
            set => _failedFiles = value;
        }

        public int TotalFiles { get; }

        public ProgressReport(int totalFiles)
        {
            TotalFiles = totalFiles;
        }
        
        public int IncrementFailedFiles()
        {
            return Interlocked.Increment(ref _failedFiles);
        }

        public int IncrementModifiedFiles()
        {
            return Interlocked.Increment(ref _modifiedFiles);
        }
    }
}