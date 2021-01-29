namespace MappingFramework.MappingInterface.Controls
{
    public class ListOfTEntry
    {
        private readonly EventList _eventList;
        private readonly ListOfTEntries _entries;
        private int _index;

        public ListOfTEntry(EventList eventList, ListOfTEntries entries, int index)
        {
            _eventList = eventList;
            _entries = entries;
            _index = index;
        }

        public void Update(object newItem) 
            => _eventList[_index] = newItem;

        public void UpdateIndex(int index) 
            => _index = index;

        public object Value() 
            => _eventList[_index];
        
        public void Remove()
        {
            _eventList.RemoveAt(_index);
            _entries.Remove(this);
        }
    }
}