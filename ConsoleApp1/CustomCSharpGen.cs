using System.Collections.Generic;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration;
using NSwag.CodeGeneration.CSharp;

namespace ConsoleApp1
{
    public class CustomCSharpGen : SwaggerToCSharpClientGenerator
    {
        public CustomCSharpGen(SwaggerDocument document, SwaggerToCSharpClientGeneratorSettings settings) : base(document, settings)
        {
        }

        public CustomCSharpGen(SwaggerDocument document, SwaggerToCSharpClientGeneratorSettings settings, CSharpTypeResolver resolver) : base(document, settings, resolver)
        {

        }

        public override string GenerateFile()
        {
            return base.GenerateFile();
        }

        public IEnumerable<string> GenerateFiles()
        {

        }
    }
}
