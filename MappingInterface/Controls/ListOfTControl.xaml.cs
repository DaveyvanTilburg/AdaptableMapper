﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using MappingFramework.Configuration;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class ListOfTControl : UserControl
    {
        private readonly InterfaceRequirement _interfaceRequirement;
        
        private readonly List<ListOfTEntry> _entries;

        private IList _list;

        public ListOfTControl(
            InterfaceRequirement interfaceRequirement)
        {
            _interfaceRequirement = interfaceRequirement;

            _entries = new List<ListOfTEntry>();

            Initialized += Load;
            InitializeComponent();

            AddButton.Click += OnAddConditionClick;
        }
        
        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _interfaceRequirement.Name();
            
            _list = (IList)Activator.CreateInstance(_interfaceRequirement.PropertyType());
            _interfaceRequirement.Update(_list);
        }
        
        private void OnAddConditionClick(object o, EventArgs e)
        {
            _list?.Add(null);

            var newEntry = new ListOfTEntry(_list, _entries, _list.Count - 1);
            _entries.Add(newEntry);

            UserControl userControl = UserControl(newEntry.Update);
            var removeAbleEntry = new ListOfTEntryControl(newEntry.Remove, userControl, _interfaceRequirement.PropertyType().GetGenericArguments().First().Name);
            StackPanelComponent.Children.Add(removeAbleEntry);
        }

        private UserControl UserControl(Action<object> updateAction)
        {
            Type type = _interfaceRequirement.PropertyType().GetGenericArguments().First();

            if (type == typeof(AdditionalSource))
            {
                var newValue = new AdditionalSourceList();
                updateAction(newValue);
                return new GenericControl(newValue, false);
            }

            if (type.IsInterface)
                return new SelectionControl(updateAction, _interfaceRequirement.PropertyType().GetGenericArguments().First().Name, type);

            if (type.IsClass)
            {
                var newValue = Activator.CreateInstance(type);
                updateAction(newValue);
                return new GenericControl(newValue, false);
            }

            throw new Exception($"Type is not supported: {type}");
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