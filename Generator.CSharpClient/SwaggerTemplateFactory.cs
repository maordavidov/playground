using NJsonSchema.CodeGeneration;
using NSwag;
using NSwag.CodeGeneration.CSharp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{

    public class SwaggerTemplateFactory : ITemplateFactory
    {
        private ITemplateSink _sink;
        private SwaggerDocument _swaggerDoc;
        private string _subsystem;
        private string _serviceName;

        public SwaggerTemplateFactory(SwaggerDocument swaggerDoc, ITemplateSink sink, string subsystem, string serviceName)
        {
            _swaggerDoc = swaggerDoc;
            _sink = sink;
            this._subsystem = subsystem;
            this._serviceName = serviceName;
        }

        public ITemplate CreateTemplate(string language, string template, object model)
        {
            if(model is CSharpClientTemplateModel)
            {
                var visitor = new SwaggerVisitor().Visit(_swaggerDoc);
                
                return new TempalteRenderer(_sink, 
                                            _subsystem, 
                                            _serviceName, 
                                            visitor.Tags, 
                                            visitor.Definitions, 
                                            (CSharpClientTemplateModel) model);
            }

            return new NullTemplateRenderer();
        }
    }

    public class NullTemplateRenderer : ITemplate
    {
        public string Render()
        {
            return "";
        }
    }
}
