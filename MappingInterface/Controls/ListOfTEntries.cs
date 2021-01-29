using System.Collections.Generic;

namespace MappingFramework.MappingInterface.Controls
{
    public class ListOfTEntries
    {
        private readonly EventList _eventList;
        private readonly List<ListOfTEntry> _entries;

        public ListOfTEntries(EventList eventList)
        {
            _eventList = eventList;
            _entries = new List<ListOfTEntry>();
        }

        public ListOfTEntry New()
        {
            var result = new ListOfTEntry(_eventList, this, _eventList.Count());
            _entries.Add(result);

            _eventList.Add(null);

            return result;
        }

        public ListOfTEntry Load(int index)
        {
            var result = new ListOfTEntry(_eventList, this, index);
            _entries.Add(result);

            return result;
        }
        
        public void Remove(ListOfTEntry tEntry)
        {
            _entries.Remove(tEntry);
            foreach (ListOfTEntry entry in _entries)
                entry.UpdateIndex(_entries.IndexOf(entry));
        }
    }
}