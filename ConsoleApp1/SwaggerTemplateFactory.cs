using NJsonSchema.CodeGeneration;
using NSwag;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{

    public class SwaggerTemplateFactory : ITemplateFactory
    {
        private SwaggerDocument _swaggerDoc;
        private string _subsystem;
        private string _serviceName;

        public SwaggerTemplateFactory(SwaggerDocument swaggerDoc, string subsystem, string serviceName)
        {
            _swaggerDoc = swaggerDoc;
            this._subsystem = subsystem;
            this._serviceName = serviceName;
        }

        public ITemplate CreateTemplate(string language, string template, object model)
        {
            var tags = new SwaggerVisitor().Visit(_swaggerDoc).Tags;
            return new TempalteRenderer(_subsystem, _serviceName, tags, model);
        }
    }
}
