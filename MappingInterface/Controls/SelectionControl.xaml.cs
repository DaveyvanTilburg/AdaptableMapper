using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class SelectionControl : UserControl
    {
        private readonly Action<object> _updateAction;
        private readonly string _name;
        private readonly Type _type;

        public SelectionControl(Action<object> updateAction, string name, Type type)
        {
            _updateAction = updateAction;
            _name = name;
            _type = type;

            Initialized += Load;
            InitializeComponent();

            SelectionComboBox.SelectionChanged += ComboBoxChanged;
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _name;

            List<string> options = OptionLists.List(_type, ContentType()).Select(t => t.Name).ToList();
            
            if (options.Count > 1)
                options.Insert(0, " ");

            foreach (string option in options)
                SelectionComboBox.Items.Add(option);

            SelectionComboBox.SelectedIndex = 0;
            ComboBoxChanged(null, null);
        }

        private void ComboBoxChanged(object o, EventArgs e)
        {
            string selectedValue = SelectionComboBox.SelectedItem.ToString() ?? string.Empty;
            
            if(string.IsNullOrWhiteSpace(selectedValue))
            {
                _updateAction(null);
                StackPanelComponent.Children.Clear();
            }
            else
            {
                Type valueType = OptionLists.List(_type, ContentType()).FirstOrDefault(t => t.Name.Equals(selectedValue, StringComparison.OrdinalIgnoreCase));
                object value = Activator.CreateInstance(valueType);
                _updateAction(value);

                StackPanelComponent.Children.Clear();
                StackPanelComponent.Children.Add(new ComponentControl(value, false));
            }
        }
        
        private ContentType ContentType()
        {
            ContextTypeAttribute attribute = (ContextTypeAttribute)_type.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(ContextTypeAttribute));
            ContextType contextType = attribute?.ContextType == ContextType.Target ? ContextType.Target : ContextType.Source;
            ContentType contentType = contextType == ContextType.Source ? MappingWindow.SourceType : MappingWindow.TargetType;

            return contentType;
        }
    }
}