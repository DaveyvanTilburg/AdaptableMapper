using System;
using System.Windows.Controls;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class ListOfTEntryControl : UserControl
    {
        private readonly Action _remove;
        private readonly UserControl _child;
        private readonly string _name;
        
        public ListOfTEntryControl(Action remove, UserControl child, string name)
        {
            _remove = remove;
            _child = child;
            _name = name;

            Initialized += Load;
            InitializeComponent();

            RemoveButton.Click += OnRemoveClick;
        }
        
        private void Load(object o, EventArgs e)
        {
            StackPanelComponent.Children.Add(_child);
            LabelComponent.Content = _name;
        }
        
        private void OnRemoveClick(object o, EventArgs e)
        {
            _remove();
            ((Panel)Parent).Children.Remove(this);
        }
    }
}