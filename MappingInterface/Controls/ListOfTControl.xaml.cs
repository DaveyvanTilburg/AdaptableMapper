using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class ListOfTControl : UserControl
    {
        private readonly List<ListOfTEntry> _entries;
        private readonly Type _defaultType;
        private readonly IList _list;
        private readonly Func<Action<object>, string, ContentType, object, UserControl> _createUserControl;
        private readonly ContentType _contentType;
        private readonly string _name;
        
        public ListOfTControl(Action<object> update, Type listType, string name, Func<Action<object>, string, ContentType, object, UserControl> createUserControl, Type defaultType, ContentType contentType)
        {
            _entries = new List<ListOfTEntry>();
            _defaultType = defaultType;
            _createUserControl = createUserControl;
            _contentType = contentType;
            _name = name;

            InitializeComponent();

            _list = (IList)Activator.CreateInstance(listType);
            update(_list);

            AddButton.Click += OnAddConditionClick;
        }
        
        private void OnAddConditionClick(object o, EventArgs e)
        {
            object newListItem = Activator.CreateInstance(_defaultType);
            _list?.Add(newListItem);

            var newEntry = new ListOfTEntry(_list, _entries, _list.Count - 1);
            _entries.Add(newEntry);

            UserControl userControl = _createUserControl(newEntry.Update, _name, _contentType, newListItem);
            var removeAbleEntry = new ListOfTEntryControl(newEntry.Remove, userControl);
            StackPanelComponent.Children.Add(removeAbleEntry);
        }
        
        private class ListOfTEntry
        {
            private readonly IList _list;
            private readonly List<ListOfTEntry> _entries;
            private int _index;

            public ListOfTEntry(IList list, List<ListOfTEntry> entries, int index)
            {
                _list = list;
                _entries = entries;
                _index = index;
            }

            public void Update(object newItem) => _list[_index] = newItem;

            public void UpdateIndex(int index) => _index = index;
            
            public void Remove()
            {
                _list.RemoveAt(_index);
                _entries.Remove(this);

                foreach (ListOfTEntry entry in _entries)
                    entry.UpdateIndex(_entries.IndexOf(entry));
            }
        }
    }
}