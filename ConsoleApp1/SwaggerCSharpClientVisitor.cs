using NSwag.CodeGeneration.CSharp.Models;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class SwaggerCSharpClientVisitor
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
            Visit(op.Path, op.PathParameters, op.QueryParameters);
        }

        private void Visit(string path, IEnumerable<CSharpParameterModel> pathParameters, IEnumerable<CSharpParameterModel> queryParameters)
        {

        }
    }
}
