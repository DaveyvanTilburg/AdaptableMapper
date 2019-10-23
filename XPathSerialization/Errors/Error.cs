using System;

namespace XPathSerialization.Errors
{
    public class Error : EventArgs
    {
        public string Message { get; }

        internal Error(string message)
        {
            Message = message;
        }
    }
}