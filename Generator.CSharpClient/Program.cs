using McMaster.Extensions.CommandLineUtils;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program
    {
        [Option("-s|--subsystem", CommandOptionType.SingleValue)]
        public string Subsystem { get; set; }
        [Option("-n|--serviceName", CommandOptionType.SingleValue)]
        public string ServiceName { get; set; }
        [Option("-o|--output", CommandOptionType.SingleValue)]
        public string Output { get; set; }
        [Option("--swagger", CommandOptionType.SingleValue)]
        public string Swagger { get; set; }

        private static void Main(string[] args)
        {
            CommandLineApplication.Execute<Program>(args);
        }

        public void OnExecute()
        {
            GenerateAsync().Wait();
        }

        public async Task GenerateAsync()
        {
            //var httpClient = new System.Net.Http.HttpClient();
            
            //string content = await httpClient.GetStringAsync("https://qa.trunovate.com/entitymanager/swagger/1.1.0.0/swagger.json");

            string content = File.ReadAllText(Swagger);

            var dir = Path.GetFullPath(Output);

            var fileSink = new FileSink(dir);

            var document = await SwaggerDocument.FromJsonAsync(content);

            var settings = new SwaggerToCSharpClientGeneratorSettings
            {
                CSharpGeneratorSettings = {

                    TemplateFactory = new SwaggerTemplateFactory(document,fileSink, Subsystem, ServiceName)
                }
            };

            var generator = new SwaggerToCSharpClientGenerator(document, settings);
            _ = generator.GenerateFile();

            Console.WriteLine(dir);
        }
    }
}
