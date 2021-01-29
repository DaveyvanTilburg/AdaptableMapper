using System;
using System.Windows.Controls;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class ListOfTControl : UserControl
    {
        private readonly IObjectLink _objectLink;
        private readonly ControlList _tEntries;
        
        public ListOfTControl(IObjectLink objectLink)
        {
            _objectLink = objectLink;
            
            _tEntries = new ControlList(objectLink, OnListUpdated);

            Initialized += Load;
            InitializeComponent();

            AddButton.Click += OnAddButtonClick;
        }
        
        private void Load(object o, EventArgs e)
        {
            UpdateName(0);

            foreach (ListOfTEntryControl control in _tEntries.Controls())
                StackPanelComponent.Children.Add(control);
        }
        
        private void OnAddButtonClick(object o, EventArgs e)
        {
            ListOfTEntryControl control = _tEntries.New();
            StackPanelComponent.Children.Add(control);
        }

        private void OnListUpdated(object o, ListUpdatedEventArgs e)
            => UpdateName(e.Count);
        
        private void UpdateName(int listCount)
            => LabelComponent.Content = $"{_objectLink.Name()} ({listCount})";
    }
}