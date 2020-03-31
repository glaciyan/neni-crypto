using Serilog;

namespace crypto.Desktop.Cnsl
{
    public class NewProject : Command
    {
        public NewProject(string name, string path)
        {
            Name = name;
            Path = path;
        }
        // new -> uses current directory
        // new name -> creates a new directory with that name and uses that
        // new name path -> goes to that path and creates a new directory with the given name and uses that

        public string Name { get; }
        public string Path { get; }

        public override void Run()
        {
            Log.Debug("Running NewProject with " +
                      $"Name = {Name ?? "null"}, Path = {Path ?? "null"}");
        }
    }
}