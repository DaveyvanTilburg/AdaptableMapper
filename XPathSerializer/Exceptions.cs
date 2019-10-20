using System;

namespace XPathSerialization
{
    public class InvalidAdaptablePathException : Exception
    {
        public InvalidAdaptablePathException(string message) : base(message) { }
    }
}