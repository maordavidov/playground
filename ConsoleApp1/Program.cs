using Infra.Http;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using System;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {


        private static void Main(string[] args)
        {
            GenerateAsync();

            Console.ReadLine();
        }

        public static async Task GenerateAsync()
        {
            var httpClient = new System.Net.Http.HttpClient();

            string content = await httpClient.GetStringAsync("https://qa.trunovate.com/entitymanager/swagger/1.1.0.0/swagger.json");
            string theNamespace = "PlantSharp.EntityManager.WebApi.Client";

            string subsystem = "PlantSharp";
            string serviceName = "EntityManager";
            string version = "1.1.0.0";

            var document = await SwaggerDocument.FromJsonAsync(content);

            var settings = new SwaggerToCSharpClientGeneratorSettings
            {
                CSharpGeneratorSettings = {
                    
                    Namespace = theNamespace,
                    TemplateFactory = new SwaggerTemplateFactory(document, subsystem, serviceName)
                }
            };

            var generator = new SwaggerToCSharpClientGenerator(document, settings);
            string code = generator.GenerateFile();

            Console.WriteLine(code);
        }
    }
}
