using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class ConditionControl : UserControl
    {
        private readonly Action<object> _assignValue;
        private readonly string _name;
        private readonly ContentType _contentType;
        
        public ConditionControl(Action<object> assignValue, string name,  ContentType contentType)
        {
            _assignValue = assignValue;
            _name = name;
            _contentType = contentType;

            Initialized += Load;
            InitializeComponent();

            ConditionsComboBox.SelectionChanged += SetValueComboBoxChanged;
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _name;

            IEnumerable<string> options = OptionLists.Conditions().Select(t => t.GetType().Name);

            foreach (string option in options)
                ConditionsComboBox.Items.Add(option);
        }

        private void SetValueComboBoxChanged(object o, EventArgs e)
        {
            string selectedValue = ConditionsComboBox.SelectedItem.ToString();

            object value = OptionLists.Conditions().FirstOrDefault(t => t.GetType().Name.Equals(selectedValue, StringComparison.OrdinalIgnoreCase));
            _assignValue(value);

            ConditionStackPanelComponent.Children.Clear();
            ConditionStackPanelComponent.Children.Add(new GenericControl(value, _contentType));
        }
    }
}