using System;

namespace MappingFramework.MappingInterface
{
    public interface Publisher<T> where T : EventArgs
    {
        public event EventHandler<T> UpdateEvent;
    }
}