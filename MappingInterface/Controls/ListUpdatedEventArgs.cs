using System;

namespace MappingFramework.MappingInterface.Controls
{
    public class ListUpdatedEventArgs : EventArgs
    {
        public int Count { get; }

        public ListUpdatedEventArgs(int count)
        {
            Count = count;
        }
    }
}