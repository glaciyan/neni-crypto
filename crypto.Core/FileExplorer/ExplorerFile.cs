using System.Diagnostics;

namespace crypto.Core.FileExplorer
{
    public class ExplorerFile : ExplorerItem
    {
        private string ExecutionPath { get; }

        public ExplorerFile(string name, string executionPath = null) : base(name)
        {
            ExecutionPath = executionPath;
        }

        public override ExplorerItem Go()
        {
            if (ExecutionPath == null) return null;
            
            var pInfo = new ProcessStartInfo {FileName = ExecutionPath, UseShellExecute = true};
            Process.Start(pInfo);

            return null;
        }
    }
}