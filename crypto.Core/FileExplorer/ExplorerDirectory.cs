using System.Collections.Generic;

namespace crypto.Core.FileExplorer
{
    public class ExplorerDirectory : ExplorerItem
    {
        private List<ExplorerItem> Children { get; } = new List<ExplorerItem>();

        public ExplorerDirectory(string name) : base(name)
        {
        }

        public void AddChild(ExplorerItem item)
        {
            AddIgnoreDupes(item, this);
        }

        private static void AddIgnoreDupes(ExplorerItem toAdd, ExplorerDirectory directory)
        {
            var childWithSameName = directory.Children.Find(c => c.Name == toAdd.Name);

            if (childWithSameName == null)
            {
                directory.Children.Add(toAdd);
                return;
            }
            
            if (toAdd is ExplorerFile)
            {
                if (childWithSameName is ExplorerDirectory)
                {
                    directory.Children.Add(toAdd);
                    return;
                }
                
                return;
            }
            
            if (toAdd is ExplorerDirectory dir)
            {
                foreach (var dirChild in dir.Children)
                {
                    AddIgnoreDupes(dirChild, childWithSameName as ExplorerDirectory);
                }
            }
        }

        public override ExplorerItem Go()
        {
            return this;
        }
    }
}