using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class ConditionControl : UserControl
    {
        private readonly PropertyInfo _valueMutation;
        private readonly object _propertyOwner;
        private readonly ContentType _contentType;
        public ConditionControl(PropertyInfo valueMutation, object propertyOwner, ContentType contentType)
        {
            _valueMutation = valueMutation;
            _propertyOwner = propertyOwner;
            _contentType = contentType;

            Initialized += Load;
            InitializeComponent();

            ConditionsComboBox.SelectionChanged += SetValueComboBoxChanged;
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _valueMutation.Name;

            IEnumerable<string> options = OptionLists.Conditions().Select(t => t.GetType().Name);

            foreach (string option in options)
                ConditionsComboBox.Items.Add(option);
        }

        private void SetValueComboBoxChanged(object o, EventArgs e)
        {
            string selectedValue = ConditionsComboBox.SelectedItem.ToString();

            object value = OptionLists.Conditions().FirstOrDefault(t => t.GetType().Name.Equals(selectedValue, StringComparison.OrdinalIgnoreCase));
            _valueMutation.SetValue(_propertyOwner, value);

            ConditionStackPanelComponent.Children.Clear();
            ConditionStackPanelComponent.Children.Add(new GenericControl(value, _contentType));
        }
    }
}