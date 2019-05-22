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
        private ITemplateSink _sink;
        private string _subsystem;
        private string _serviceName;
        private (string Path, string HttpMethod, string Tag)[] _tags;
        private string[] _definitions;

        public TempalteRenderer(ITemplateSink sink, 
                                string subsystem, 
                                string serviceName, 
                                (string Path, string HttpMethod, string Tag)[] tags,
                                string[] definitions,
                                CSharpClientTemplateModel model)
        {
            _sink = sink;
            _subsystem = subsystem;
            _serviceName = serviceName;
            _tags = tags;
            _definitions = definitions;
            this._model = model;
            _stuble = new StubleRenderer();
        }

        public string Render()
        {
            SetupRootFolder();
          
            var operationsByTags = new SwaggerCSharpClientVisitor(_tags, _definitions).Visit(_model).Operations;

            foreach ((string templateFile, string name) in TemplateFiles())
            {
                string template = File.ReadAllText(Path.Combine("Templates", templateFile));
                if (template.Contains("Client"))
                {
                    RenderClient(operationsByTags, name, template);
                }
                else
                {
                    RenderGlobal(name, template);
                }
            }

            return "";
        }

        private void SetupRootFolder()
        {
            var content = new
            {
                Subsystem = _subsystem,
                ServiceName = _serviceName
            };

            string folder = "{{Subsystem}}.{{ServiceName}}.WebApi.Client";
            folder = _stuble.RenderAsync(folder, content).Result;
            _sink.SetupRoot(folder);
        }

        private void RenderGlobal(string name, string template)
        {
            var content = new
            {
                Subsystem = _subsystem,
                ServiceName = _serviceName
            };

            string theName = _stuble.RenderAsync(name, content).Result;
            string theContent = _stuble.RenderAsync(template, content).Result;

            _sink.Receive(theName, theContent);
        }

        private void RenderClient(ILookup<string, object> operationsByTags, string name, string template)
        {
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

                string theName = _stuble.RenderAsync(name, content).Result;
                string theContent = _stuble.RenderAsync(template, content).Result;

                _sink.Receive(theName, theContent);
            }
        }

        private IEnumerable<(string templateFile, string name)> TemplateFiles()
        {
            yield return ("Client.DI.mustache", "{{Tag}}Client.DI.cs");
            yield return ("IClient.mustache", "I{{Tag}}Client.cs");
            yield return ("Client.mustache", "{{Tag}}Client.cs");
            yield return ("csproj.mustache", "{{Subsystem}}.{{ServiceName}}.WebApi.Client.csproj");
            yield return ("healthchecks.mustache", "{{ServiceName}}HealthChecks.cs");
        }
    }
}
