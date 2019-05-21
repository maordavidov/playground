using NSwag.CodeGeneration.CSharp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class SwaggerVisitor
    {
        public void Visit(CSharpClientTemplateModel model)
        {
            Visit(model.Operations);
        }

        protected virtual void Visit(IEnumerable<CSharpOperationModel> operations)
        {
            foreach (var op in operations)
            {
                Visit(op);
            }
        }

        protected virtual void Visit(CSharpOperationModel op)
        {
            
        }
    }
}
