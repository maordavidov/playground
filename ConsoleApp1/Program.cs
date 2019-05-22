using NSwag;
using NSwag.CodeGeneration.CSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {

        
        private static void Main(string[] args)
        {
            GenerateAsync().Wait();

            Console.ReadLine();
        }

        public static async Task GenerateAsync()
        {
            var httpClient = new System.Net.Http.HttpClient();
            
            string content = await httpClient.GetStringAsync("https://qa.trunovate.com/apigateway/swagger/1.0.0.0/swagger.json");
            string theNamespace = "PlantSharp.EntityManager.WebApi.Client";

            string subsystem = "PlantSharp";
            string serviceName = "EntityManager";
            string version = "1.1.0.0";
            var dir = Path.GetFullPath("./proj");

            var fileSink = new FileSink(dir);

            var document = await SwaggerDocument.FromJsonAsync(content);

            var settings = new SwaggerToCSharpClientGeneratorSettings
            {
                CSharpGeneratorSettings = {

                    Namespace = theNamespace,
                    TemplateFactory = new SwaggerTemplateFactory(document,fileSink, subsystem, serviceName)
                }
            };

            var generator = new SwaggerToCSharpClientGenerator(document, settings);
            _ = generator.GenerateFile();

            Console.WriteLine(dir);
        }
    }
}
