using System;

namespace MappingFramework.Process
{
    public sealed class Information : EventArgs
    {
        public string Message { get; }
        public string Type { get; }

        public Information(string message, string type)
        {
            Message = message;
            Type = type;
        }
    }
}