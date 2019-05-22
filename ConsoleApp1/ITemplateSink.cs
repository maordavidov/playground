using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp1
{
    public interface ITemplateSink
    {
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
    }
}
