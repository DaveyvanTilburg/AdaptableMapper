using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class ValueMutationControl : UserControl
    {
        private readonly Action<object> _assignValue;
        private readonly string _name;
        public ValueMutationControl(Action<object> assignValue, string name)
        {
            _assignValue = assignValue;
            _name = name;

            Initialized += Load;
            InitializeComponent();

            ValueMutationComboBox.SelectionChanged += SetValueComboBoxChanged;
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _name;

            IEnumerable<string> options = OptionLists.ValueMutations().Select(t => t.GetType().Name);

            foreach (string option in options)
                ValueMutationComboBox.Items.Add(option);
        }

        private void SetValueComboBoxChanged(object o, EventArgs e)
        {
            string selectedValue = ValueMutationComboBox.SelectedItem.ToString();

            object value = OptionLists.ValueMutations().FirstOrDefault(t => t.GetType().Name.Equals(selectedValue, StringComparison.OrdinalIgnoreCase));
            _assignValue(value);

            ValueMutationStackPanelComponent.Children.Clear();
            ValueMutationStackPanelComponent.Children.Add(new GenericControl(value));
        }
    }
}