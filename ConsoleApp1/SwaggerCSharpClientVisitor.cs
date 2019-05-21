using NSwag.CodeGeneration.CSharp.Models;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public class SwaggerCSharpClientVisitor
    {
        private (string Path, string HttpMethod, string Tag)[] _tags;
        private List<(string Tag, CSharpOperationModel Operation)> _operations;

        public SwaggerCSharpClientVisitor((string Path, string HttpMethod, string Tag)[] tags)
        {
            _tags = tags;
        }

        public SwaggerCSharpClientVisitor Visit(CSharpClientTemplateModel model)
        {
            _operations = new List<(string, CSharpOperationModel)>();
            Visit(model.Operations);
            return this;
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
            string method = op.HttpMethodLower;
            string path = op.Path;

            string tag = _tags.FindTag(method, path);

            _operations.Add((tag, op));

            Visit(op.Path, op.PathParameters, op.QueryParameters);
        }

        private void Visit(string path, IEnumerable<CSharpParameterModel> pathParameters, IEnumerable<CSharpParameterModel> queryParameters)
        {

        }

        public ILookup<string, CSharpOperationModel> Operations => _operations.ToLookup(k => k.Tag, k => k.Operation);
    }
}
