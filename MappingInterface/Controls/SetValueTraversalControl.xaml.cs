using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class SetValueTraversalControl : UserControl
    {
        private readonly Action<object> _assignValue;
        private readonly string _name;
        private readonly ContentType _contentType;

        public SetValueTraversalControl(Action<object> assignValue, string name, ContentType contentType)
        {
            _assignValue = assignValue;
            _name = name;
            _contentType = contentType;

            Initialized += Load;
            InitializeComponent();

            SetValueTraversalComboBox.SelectionChanged += SetValueComboBoxChanged;
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _name;
            
            IEnumerable<string> options = OptionLists.SetValueTraversals(_contentType).Select(t => t.GetType().Name);

            foreach (string option in options)
                SetValueTraversalComboBox.Items.Add(option);
        }

        private void SetValueComboBoxChanged(object o, EventArgs e)
        {
            string selectedValue = SetValueTraversalComboBox.SelectedItem.ToString();

            object value = OptionLists.SetValueTraversals(_contentType).FirstOrDefault(t => t.GetType().Name.Equals(selectedValue, StringComparison.OrdinalIgnoreCase));
            _assignValue(value);

            SetValueStackPanelComponent.Children.Clear();
            SetValueStackPanelComponent.Children.Add(new GenericControl(value, _contentType));
        }
    }
}