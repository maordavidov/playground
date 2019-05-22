using NSwag;
using NSwag.CodeGeneration.CSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program
    {
        private static void Main(string[] args)
        {
            GenerateAsync(args[0], args[1], args[2]).Wait();
        }

        public static async Task GenerateAsync(string subsystem, string serviceName, string output)
        {
            var httpClient = new System.Net.Http.HttpClient();
            
            string content = await httpClient.GetStringAsync("https://qa.trunovate.com/entitymanager/swagger/1.1.0.0/swagger.json");
            var dir = Path.GetFullPath(output);

            var fileSink = new FileSink(dir);

            var document = await SwaggerDocument.FromJsonAsync(content);

            var settings = new SwaggerToCSharpClientGeneratorSettings
            {
                CSharpGeneratorSettings = {

                    TemplateFactory = new SwaggerTemplateFactory(document,fileSink, subsystem, serviceName)
                }
            };

            var generator = new SwaggerToCSharpClientGenerator(document, settings);
            _ = generator.GenerateFile();

            Console.WriteLine(dir);
        }
    }
}
