using NSwag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    public class SwaggerVisitor
    {
        private List<(string, string, string)> _tags;

        public SwaggerVisitor Visit(SwaggerDocument document)
        {
            _tags = new List<(string, string, string)>();

            foreach (var path in document.Paths)
            {
                Visit(path);
            }

            return this;
        }

        protected virtual void Visit(KeyValuePair<string, SwaggerPathItem> path)
        {
            var pathItem = path.Value;

            foreach (KeyValuePair<string, SwaggerOperation> pair in pathItem)
            {
                SwaggerOperation op = pair.Value;
                string tag = op.Tags.Single();

                _tags.Add((path.Key, pair.Key, tag));
            }
        }

        public (string Path, string HttpMethod, string Tag)[] Tags => _tags.ToArray();
    }
}
