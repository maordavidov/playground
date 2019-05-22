using NSwag.CodeGeneration.CSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    public class SwaggerCSharpClientVisitor
    {
        private (string Path, string HttpMethod, string Tag)[] _tags;
        private List<(string Tag, object Operation)> _operations;

        public SwaggerCSharpClientVisitor((string Path, string HttpMethod, string Tag)[] tags)
        {
            _tags = tags;
        }

        public SwaggerCSharpClientVisitor Visit(CSharpClientTemplateModel model)
        {
            _operations = new List<(string, object)>();
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

            string tag = _tags.FindTag(method, op.Path);

            string path = ModifyPath(op.Path);

            var body = op.Parameters.FirstOrDefault(p => p.Kind == NSwag.SwaggerParameterKind.Body);

            var theOp = new
            {
                op.Id,
                Name = op.ActualOperationName,
                ResultType = op.SyncResultType,
                Path = path,
                HttpMethod = op.HttpMethodUpper,
                IsVoid = op.SyncResultType.Equals("void"),
                HasInput = op.HasBody || op.HasQueryParameters || op.PathParameters.Any(),
                HasBody = op.HasBody,
                BodyType = body?.Type,
                NotSupported = op.HasFormParameters
            };

            

            

            _operations.Add((tag, theOp));
            
            Visit(op.Path, op.PathParameters, op.QueryParameters);
        }

        private string ModifyPath(string path)
        {
            var matches = Regex.Matches(path, "{(?<PathParam>.+?)}");

            foreach (Match match in matches)
            {
                if(match.Success == false)
                {
                    continue;
                }

                string pathParam = match.Groups["PathParam"].Value;
                path = path.Replace(pathParam, $"request.{pathParam}");
            }


            return path;
        }

        private void Visit(string path, IEnumerable<CSharpParameterModel> pathParameters, IEnumerable<CSharpParameterModel> queryParameters)
        {

        }

        public ILookup<string, object> Operations => _operations.ToLookup(k => k.Tag, k => k.Operation);
    }
}
