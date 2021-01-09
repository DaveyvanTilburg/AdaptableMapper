using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class GetValueTraversalControl : UserControl
    {
        private readonly Action<object> _assignValue;
        private readonly string _name;
        private readonly ContentType _contentType;

        public GetValueTraversalControl(Action<object> assignValue, string name, ContentType contentType)
        {
            _assignValue = assignValue;
            _name = name;
            _contentType = contentType;

            Initialized += Load;
            InitializeComponent();

            GetValueTraversalComboBox.SelectionChanged += GetValueComboBoxChanged;
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _name;
            
            IEnumerable<string> options = OptionLists.GetValueTraversals(_contentType).Select(t => t.GetType().Name);

            foreach (string option in options)
                GetValueTraversalComboBox.Items.Add(option);
        }

        private void GetValueComboBoxChanged(object o, EventArgs e)
        {
            string selectedValue = GetValueTraversalComboBox.SelectedItem.ToString();

            object value = OptionLists.GetValueTraversals(_contentType).FirstOrDefault(t => t.GetType().Name.Equals(selectedValue, StringComparison.OrdinalIgnoreCase));
            _assignValue(value);

            GetValueStackPanelComponent.Children.Clear();
            GetValueStackPanelComponent.Children.Add(new GenericControl(value, _contentType));
        }
    }
}