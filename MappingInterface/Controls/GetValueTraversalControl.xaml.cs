using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class GetValueTraversalControl : UserControl
    {
        private readonly PropertyInfo _getValueTraversal;
        private readonly object _propertyOwner;
        private readonly ContentType _sourceType;

        public GetValueTraversalControl(PropertyInfo getValueTraversal, object propertyOwner, ContentType sourceType)
        {
            _getValueTraversal = getValueTraversal;
            _propertyOwner = propertyOwner;
            _sourceType = sourceType;

            Initialized += Load;
            InitializeComponent();

            GetValueTraversalComboBox.SelectionChanged += GetValueComboBoxChanged;
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _getValueTraversal.Name;
            
            IEnumerable<string> getValueTraversalOptions = OptionLists.GetValueTraversals(_sourceType).Select(t => t.GetType().Name);

            foreach (string option in getValueTraversalOptions)
                GetValueTraversalComboBox.Items.Add(option);
        }

        private void GetValueComboBoxChanged(object o, EventArgs e)
        {
            string selectedValue = GetValueTraversalComboBox.SelectedItem.ToString();

            object value = OptionLists.GetValueTraversals(_sourceType).FirstOrDefault(t => t.GetType().Name.Equals(selectedValue, StringComparison.OrdinalIgnoreCase));
            _getValueTraversal.SetValue(_propertyOwner, value);

            GetValueStackPanelComponent.Children.Clear();
            GetValueStackPanelComponent.Children.Add(new GenericControl(value, _sourceType));
        }
    }
}