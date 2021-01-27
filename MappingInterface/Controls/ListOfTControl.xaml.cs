using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class ListOfTControl : UserControl
    {
        private readonly IObjectLink _objectLink;
        
        private readonly List<ListOfTEntry> _entries;

        private IList _list;
        private event EventHandler ListUpdated;

        public ListOfTControl(IObjectLink objectLink)
        {
            _objectLink = objectLink;

            _entries = new List<ListOfTEntry>();

            Initialized += Load;
            InitializeComponent();

            AddButton.Click += OnAddClick;
            ListUpdated += OnListUpdated;
        }
        
        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = $"{_objectLink.Name()} (0)";

            object currentValue = _objectLink.Value();
            if (currentValue != null)
            {
                _list = (IList)currentValue;
                
                for(int index = 0; index < _list.Count; index++)
                {
                    var newEntry = new ListOfTEntry(_list, _entries, ListUpdated, index);
                    _entries.Add(newEntry);

                    var removeAbleEntry = new ListOfTEntryControl(_objectLink, newEntry);
                    StackPanelComponent.Children.Add(removeAbleEntry);
                }

                ListUpdated?.Invoke(null, EventArgs.Empty);
            }
            else
            {
                _list = (IList)Activator.CreateInstance(_objectLink.PropertyType());
                _objectLink.Update(_list);
            }
        }
        
        private void OnAddClick(object o, EventArgs e)
        {
            _list.Add(null);

            var newEntry = new ListOfTEntry(_list, _entries, ListUpdated, _list.Count - 1);
            _entries.Add(newEntry);

            var removeAbleEntry = new ListOfTEntryControl(_objectLink, newEntry);
            StackPanelComponent.Children.Add(removeAbleEntry);

            ListUpdated?.Invoke(null, EventArgs.Empty);
        }

        private void OnListUpdated(object o, EventArgs e)
        {
            LabelComponent.Content = $"{_objectLink.Name()} ({_list.Count})";
        }
    }
}