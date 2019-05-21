using NJsonSchema.CodeGeneration;
using NSwag.CodeGeneration.CSharp.Models;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    public class TempalteRenderer : ITemplate
    {
        private CSharpClientTemplateModel _model;
        private StubleRenderer _stuble;
        private string _subsystem;
        private string _serviceName;

        public TempalteRenderer(string subsystem, string serviceName, object model)
        {
            _subsystem = subsystem;
            _serviceName = serviceName;
            this._model = (CSharpClientTemplateModel)model;
            _stuble = new StubleRenderer();
        }

        public string Render()
        {
            string template = File.ReadAllText(Path.Combine("Templates", "IClient.mustache"));

            var operations = _model.Operations.Select(o => new
            {
                o.Id,
                ResultType = o.SyncResultType,
                o.Path,
                HttpMethod = o.HttpMethodUpper,
                IsVoid = o.SyncResultType.Equals("void")
            });


            var content = new
            {
                Subsystem = _subsystem,
                ServiceName = _serviceName,
                Operations = operations
            };


            new SwaggerVisitor().Visit(_model);

           string con = _stuble.RenderAsync(template, content).Result;

            return con;
        }
    }
}
