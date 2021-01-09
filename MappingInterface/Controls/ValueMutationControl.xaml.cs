using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class ValueMutationControl : UserControl
    {
        private readonly PropertyInfo _valueMutation;
        private readonly object _propertyOwner;
        public ValueMutationControl(PropertyInfo valueMutation, object propertyOwner)
        {
            _valueMutation = valueMutation;
            _propertyOwner = propertyOwner;

            Initialized += Load;
            InitializeComponent();

            ValueMutationComboBox.SelectionChanged += SetValueComboBoxChanged;
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _valueMutation.Name;

            IEnumerable<string> setValueTraversalOptions = OptionLists.ValueMutations().Select(t => t.GetType().Name);

            foreach (string option in setValueTraversalOptions)
                ValueMutationComboBox.Items.Add(option);
        }

        private void SetValueComboBoxChanged(object o, EventArgs e)
        {
            string selectedValue = ValueMutationComboBox.SelectedItem.ToString();

            object value = OptionLists.ValueMutations().FirstOrDefault(t => t.GetType().Name.Equals(selectedValue, StringComparison.OrdinalIgnoreCase));
            _valueMutation.SetValue(_propertyOwner, value);

            ValueMutationStackPanelComponent.Children.Clear();
            ValueMutationStackPanelComponent.Children.Add(new GenericControl(value));
        }
    }
}