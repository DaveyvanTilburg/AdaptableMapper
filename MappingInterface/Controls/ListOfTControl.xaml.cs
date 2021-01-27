using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class ListOfTControl : UserControl
    {
        private readonly ObjectComponentLink _objectComponentLink;
        
        private readonly List<ListOfTEntry> _entries;

        private IList _list;
        private event EventHandler ListUpdated;

        public ListOfTControl(
            ObjectComponentLink objectComponentLink)
        {
            _objectComponentLink = objectComponentLink;

            _entries = new List<ListOfTEntry>();

            Initialized += Load;
            InitializeComponent();

            AddButton.Click += OnAddConditionClick;
            ListUpdated += OnListUpdated;
        }
        
        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = $"{_objectComponentLink.Name()} (0)";
            
            _list = (IList)Activator.CreateInstance(_objectComponentLink.PropertyType());
            _objectComponentLink.Update(_list);
        }
        
        private void OnAddConditionClick(object o, EventArgs e)
        {
            _list.Add(null);

            var newEntry = new ListOfTEntry(_list, _entries, ListUpdated, _list.Count - 1);
            _entries.Add(newEntry);

            var removeAbleEntry = new ListOfTEntryControl(_objectComponentLink, newEntry);
            StackPanelComponent.Children.Add(removeAbleEntry);

            ListUpdated?.Invoke(null, EventArgs.Empty);
        }

        private void OnListUpdated(object o, EventArgs e)
        {
            LabelComponent.Content = $"{_objectComponentLink.Name()} ({_list.Count})";
        }
    }
}