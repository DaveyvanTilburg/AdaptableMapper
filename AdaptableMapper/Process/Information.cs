using System;

namespace AdaptableMapper.Process
{
    public sealed class Information : EventArgs
    {
        public string Message { get; }
        public string Type { get; }

        internal Information(string message, string type)
        {
            Message = message;
            Type = type;
        }
    }
}