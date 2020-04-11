using System;
using System.Threading.Tasks;
using crypto.Core;
using crypto.Desktop.Cnsl.Commands;

namespace crypto.Desktop.Cnsl.BrowserCommands
{
    public class GoCommandAsync : Command
    {
        private readonly VaultBrowser _browser;
        private readonly string? _indexOrBack;

        public GoCommandAsync(VaultBrowser browser, string? indexOrBack)
        {
            _browser = browser;
            _indexOrBack = indexOrBack;
        }

        private void GoToPath(string targetPath)
        {
            var splitPath = NPath.SplitPath(targetPath);

            foreach (var path in splitPath)
            {
                _browser.CurrentPathStack.Push(path);
            }
        }
        
        private void GoBack()
        {
            _browser.CurrentPathStack.TryPop(out _);
        }

        public override void Run()
        {
            if (_indexOrBack == null) throw new ArgumentException("No file index or operation given");

            switch (_indexOrBack)
            {
                case "back":
                    GoBack();
                    break;
                default:
                    var index = int.Parse(_indexOrBack);
                    break;
            }
        }
    }
}