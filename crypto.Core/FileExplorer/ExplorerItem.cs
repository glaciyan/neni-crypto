using System.Text;

namespace crypto.Core.FileExplorer
{
    public abstract class ExplorerItem
    {
        public string Name { get; set; }
        private ExplorerItem Parent { get; set; }

        protected ExplorerItem(string name)
        {
            Name = name;
        }

        public string Path
        {
            get
            {
                var path = new StringBuilder();
                var current = this;

                do
                {
                    path.Insert(0, $"/{current.Name}");
                } while (Parent != null);

                return path.ToString();
            }
        }

        public abstract ExplorerItem Go();
    }
}