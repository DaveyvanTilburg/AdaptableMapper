using System;

namespace XPathSerialization
{
    public class InvalidAdaptablePathException : Exception
    {
        public InvalidAdaptablePathException(string message) : base(message) { }
    }

    public class InvalidXPathException : Exception
    {
        public InvalidXPathException(string message) : base(message) { }
    }
}