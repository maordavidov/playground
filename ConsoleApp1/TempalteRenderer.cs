using NJsonSchema.CodeGeneration;
using NSwag.CodeGeneration.CSharp.Models;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    public class TempalteRenderer : ITemplate
    {
        private CSharpClientTemplateModel _model;
        private StubleRenderer _stuble;
        private string _subsystem;
        private string _serviceName;
        private (string Path, string HttpMethod, string Tag)[] _tags;

        public TempalteRenderer(string subsystem, string serviceName, (string Path, string HttpMethod, string Tag)[] tags, object model)
        {
            _subsystem = subsystem;
            _serviceName = serviceName;
            _tags = tags;

            this._model = (CSharpClientTemplateModel)model;
            _stuble = new StubleRenderer();
        }

        public string Render()
        {
            string template = File.ReadAllText(Path.Combine("Templates", "IClient.mustache"));
            var operationsByTags = new SwaggerCSharpClientVisitor(_tags).Visit(_model).Operations;

            foreach (IGrouping<string, CSharpOperationModel> group in operationsByTags)
            {
                string tag = group.Key;
                var operations = group.Select(o => new
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

                string con = _stuble.RenderAsync(template, content).Result;

                return con;
            }

            return "";
           
        }
    }
}
