using System;

namespace MappingFramework.MappingInterface.Identifiers
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