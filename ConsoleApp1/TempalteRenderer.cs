using NJsonSchema.CodeGeneration;
using NSwag.CodeGeneration.CSharp.Models;
using System;
using System.Collections.Generic;
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
        private string[] _files = new[] { "Client.mustache", "csproj.mustache", "healthchecks.mustache", "IClient.mustache" };
        private int _renderCount = 0;

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

            string template = File.ReadAllText(Path.Combine("Templates", "healthchecks.mustache"));
            var operationsByTags = new SwaggerCSharpClientVisitor(_tags).Visit(_model).Operations;

            foreach (IGrouping<string, object> group in operationsByTags)
            {
                string tag = group.Key;
                IEnumerable<object> operations = group;

                var content = new
                {
                    Tag = tag,
                    Subsystem = _subsystem,
                    ServiceName = _serviceName,
                    Operations = operations
                };

                string con = _stuble.RenderAsync(template, content).Result;

                return con;
            }

            _renderCount++;

            return "";
           
        }
    }
}
