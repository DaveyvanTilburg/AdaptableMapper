using System;

namespace AdaptableMapper.Errors
{
    public sealed class Error : EventArgs
    {
        public string Message { get; }

        internal Error(string message)
        {
            Message = message;
        }
    }
}