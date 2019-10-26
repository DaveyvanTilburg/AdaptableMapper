using System;

namespace AdaptableMapper
{
    public class InvalidAdaptablePathException : Exception
    {
        public InvalidAdaptablePathException(string message) : base(message) { }
    }

    public class InvalidXPathException : Exception
    {
        public InvalidXPathException(string message) : base(message) { }
    }

    public class XPathConfigurationException : Exception
    {
        public XPathConfigurationException(string message) : base(message) { }
    }
}