using System.Collections.Generic;

namespace MappingFramework.Traversals
{
    public static class StringExtensions
    {
        public static Stack<string> ToStack(this string value)
        {
            return new Stack<string>(value.Split('/'));
        }
    }
}