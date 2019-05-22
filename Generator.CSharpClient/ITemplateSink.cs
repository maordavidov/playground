using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp1
{
    public interface ITemplateSink
    {
        void SetupRoot(string root);
        void Receive(string templateName, string content);
    }

    public class FileSink : ITemplateSink
    {
        private string _dir;

        public FileSink(string directory)
        {
            Directory.CreateDirectory(directory);
            _dir = directory;
        }

        public void Receive(string name, string content)
        {
            File.WriteAllText(Path.Combine(_dir, name), content);
        }

        public void SetupRoot(string root)
        {
            _dir = Path.Combine(_dir, root);
            Directory.CreateDirectory(_dir);
        }
    }
}
