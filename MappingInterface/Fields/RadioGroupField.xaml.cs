using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Fields
{
    public partial class RadioGroupField : UserControl
    {
        private readonly InterfaceRequirement _interfaceRequirement;

        public RadioGroupField(InterfaceRequirement interfaceRequirement)
        {
            _interfaceRequirement = interfaceRequirement;

            Initialized += Load;
            InitializeComponent();
        }

        private void Load(object o, EventArgs e)
        {
            ComponentLabel.Content = _interfaceRequirement.Name();

            List<object> values = Enum.GetValues(_interfaceRequirement.PropertyType()).OfType<object>().ToList();
            
            foreach(object value in values)
            {
                var radioButton = new RadioButton
                {
                    Content = value.ToString(),
                    IsChecked = values.IndexOf(value) == 0
                };

                radioButton.Click += OnClicked;

                ComponentStackPanel.Children.Add(radioButton);
            }
        }

        private void OnClicked(object o, EventArgs e)
        {
            string value = ComponentStackPanel.Children.OfType<RadioButton>().FirstOrDefault(rb => rb.IsChecked ?? false)?.Content?.ToString() ?? string.Empty;
            object enumValue = Enum.Parse(_interfaceRequirement.PropertyType(), value);
            
            _interfaceRequirement.Update(enumValue);
        }
    }
}