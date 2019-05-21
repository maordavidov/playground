using NJsonSchema.CodeGeneration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{

    public class SwaggerTemplateFactory : ITemplateFactory
    {
        private string _subsystem;
        private string _serviceName;

        public SwaggerTemplateFactory(string subsystem, string serviceName)
        {
            this._subsystem = subsystem;
            this._serviceName = serviceName;
        }

        public ITemplate CreateTemplate(string language, string template, object model)
        {
            return new TempalteRenderer(_subsystem, _serviceName, model);
        }
    }
}
