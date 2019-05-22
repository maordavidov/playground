using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    public static class Extensions
    {
        public static string FindTag(this (string Path, string HttpMethod, string Tag)[] tags, string method, string path)
        {
            return tags.Single(t => t.Path.Equals(path) && t.HttpMethod.Equals(method)).Tag;
        }
    }
}
