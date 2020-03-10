using System.Diagnostics;

namespace crypto.Desktop.Cnsl
{
    public class NewProject : IExecutionConfig
    {
        // new -> uses current directory
        // new name -> creates a new directory with that name and uses that
        // new name path -> goes to that path and creates a new directory with the given name and uses that

        public const string Keyword = "new";

        public string Name { get; }
        public string Path { get; private set; }

        public NewProject(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public void Run()
        {
            Debug.WriteLine($"Name: {Name ?? "null"}, Path: {Path ?? "null"}");


        }
    }
}