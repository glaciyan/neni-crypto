using System;

namespace crypto.Desktop.Cnsl.BrowserCommands
{
    public class GoCommand : Command
    {
        private readonly VaultBrowser _browser;
        private readonly string? _dest;

        public GoCommand(VaultBrowser browser, string? dest)
        {
            _browser = browser;
            _dest = dest;
        }

        private void GoToPath(string targetPath)
        {
            _browser.VerifiedPathPush(targetPath);
        }
        
        private void GoBack()
        {
            _browser.CurrentPathStack.TryPop(out _);
        }

        public override void Run()
        {
            if (_dest == null) throw new ArgumentException("No file index or operation given");

            switch (_dest)
            {
                case "back":
                    GoBack();
                    break;
                default:
                    GoToPath(_dest);
                    break;
            }
        }
    }
}