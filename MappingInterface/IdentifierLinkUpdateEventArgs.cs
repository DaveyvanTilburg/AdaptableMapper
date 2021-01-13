using System;

namespace MappingFramework.MappingInterface
{
    public class IdentifierLinkUpdateEventArgs : EventArgs
    {
        public string Text { get; }

        public IdentifierLinkUpdateEventArgs(string text)
        {
            Text = text;
        }
    }
}