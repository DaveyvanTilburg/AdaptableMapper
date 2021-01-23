using System;

namespace MappingFramework.Process
{
    public sealed class Information : EventArgs
    {
        public string Message { get; }
        public InformationType Type { get; }
        public Exception Exception { get; }

        public Information(string message, InformationType type)
        {
            Message = message;
            Type = type;
        }

        public Information(string message, InformationType type, Exception exception) : this(message, type)
        {
            Exception = exception;
        }
    }
}