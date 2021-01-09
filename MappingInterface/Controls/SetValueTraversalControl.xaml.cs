using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class SetValueTraversalControl : UserControl
    {
        private readonly PropertyInfo _setValueTraversal;
        private readonly object _propertyOwner;
        private readonly ContentType _targetType;

        public SetValueTraversalControl(PropertyInfo setValueTraversal, object propertyOwner, ContentType targetType)
        {
            _setValueTraversal = setValueTraversal;
            _propertyOwner = propertyOwner;
            _targetType = targetType;

            Initialized += Load;
            InitializeComponent();

            SetValueTraversalComboBox.SelectionChanged += SetValueComboBoxChanged;
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _setValueTraversal.Name;
            
            IEnumerable<string> setValueTraversalOptions = OptionLists.SetValueTraversals(_targetType).Select(t => t.GetType().Name);

            foreach (string option in setValueTraversalOptions)
                SetValueTraversalComboBox.Items.Add(option);
        }

        private void SetValueComboBoxChanged(object o, EventArgs e)
        {
            string selectedValue = SetValueTraversalComboBox.SelectedItem.ToString();

            object value = OptionLists.SetValueTraversals(_targetType).FirstOrDefault(t => t.GetType().Name.Equals(selectedValue, StringComparison.OrdinalIgnoreCase));
            _setValueTraversal.SetValue(_propertyOwner, value);

            SetValueStackPanelComponent.Children.Clear();
            SetValueStackPanelComponent.Children.Add(new GenericControl(value, _targetType));
        }
    }
}