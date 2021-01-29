using System;
using System.Collections;

namespace MappingFramework.MappingInterface.Controls
{
    public class EventList
    {
        private readonly IList _list;
        private readonly EventHandler<ListUpdatedEventArgs> _listUpdated;
        
        public EventList(IList list, EventHandler<ListUpdatedEventArgs> subscriber)
        {
            _list = list;
            _listUpdated += subscriber;
        }
        
        public void Add(object item)
        {
            _list.Add(item);
            Trigger();
        }
        
        public object this[int index]
        {
            set => _list[index] = value;
            get => _list[index];
        }
        
        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
            Trigger();
        }
        
        public void Trigger()
            => _listUpdated?.Invoke(null, new ListUpdatedEventArgs(_list.Count));

        public int Count()
            => _list.Count;
    }
}