using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class GetValueStringTraversalControl : UserControl
    {
        private readonly Action<object> _assignValue;
        private readonly string _name;

        public GetValueStringTraversalControl(Action<object> assignValue, string name)
        {
            _assignValue = assignValue;
            _name = name;

            Initialized += Load;
            InitializeComponent();

            GetValueStringTraversalComboBox.SelectionChanged += GetValueComboBoxChanged;
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _name;
            
            IEnumerable<string> options = OptionLists.GetValueStringTraversals().Select(t => t.GetType().Name);

            foreach (string option in options)
                GetValueStringTraversalComboBox.Items.Add(option);
        }

        private void GetValueComboBoxChanged(object o, EventArgs e)
        {
            string selectedValue = GetValueStringTraversalComboBox.SelectedItem.ToString();

            object value = OptionLists.GetValueStringTraversals().FirstOrDefault(t => t.GetType().Name.Equals(selectedValue, StringComparison.OrdinalIgnoreCase));
            _assignValue(value);

            GetValueStringStackPanelComponent.Children.Clear();
            GetValueStringStackPanelComponent.Children.Add(new GenericControl(value));
        }
    }
}