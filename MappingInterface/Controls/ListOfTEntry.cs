using System;
using System.Collections;
using System.Collections.Generic;

namespace MappingFramework.MappingInterface.Controls
{
    public class ListOfTEntry
    {
        private readonly IList _list;
        private readonly List<ListOfTEntry> _entries;
        private readonly EventHandler _listUpdated;
        private int _index;

        public ListOfTEntry(IList list, List<ListOfTEntry> entries, EventHandler listUpdated, int index)
        {
            _list = list;
            _entries = entries;
            _listUpdated = listUpdated;
            _index = index;
        }

        public void Update(object newItem) => _list[_index] = newItem;

        public void UpdateIndex(int index) => _index = index;

        public object Value() => _list[_index];
        
        public void Remove()
        {
            _list.RemoveAt(_index);
            _entries.Remove(this);

            foreach (ListOfTEntry entry in _entries)
                entry.UpdateIndex(_entries.IndexOf(entry));

            _listUpdated?.Invoke(null, EventArgs.Empty);
        }
    }
}