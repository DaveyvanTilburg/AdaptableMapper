using System;

namespace MappingFramework.MappingInterface.Identifiers
{
    public interface Publisher<T> where T : EventArgs
    {
        public event EventHandler<T> UpdateEvent;
    }
}