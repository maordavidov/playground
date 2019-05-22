using Stubble.Core;
using Stubble.Core.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class StubleRenderer
    {
        private StubbleVisitorRenderer _stubble;

        public StubleRenderer()
        {
            _stubble = new StubbleBuilder().Configure(opts =>
            {
                opts.SetIgnoreCaseOnKeyLookup(true);
            }).Build();
        }

        public async Task<string> RenderAsync(string template, object content)
        {
            var settings = new Stubble.Core.Settings.RenderSettings
            {
                ThrowOnDataMiss = false
            };

            return await _stubble.RenderAsync(template, content, settings);
        }
    }
}
