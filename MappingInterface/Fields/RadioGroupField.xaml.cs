using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Fields
{
    public partial class RadioGroupField : UserControl
    {
        private readonly IObjectLink _objectLink;

        public RadioGroupField(IObjectLink objectLink)
        {
            _objectLink = objectLink;

            Initialized += Load;
            InitializeComponent();
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _objectLink.Name();

            object currentValue = _objectLink.Value();
            List<object> values = Enum.GetValues(_objectLink.PropertyType()).OfType<object>().ToList();
            
            foreach(object value in values)
            {
                var radioButton = new RadioButton
                {
                    Content = value.ToString(),
                    IsChecked = value.Equals(currentValue)
                };

                radioButton.Click += OnClicked;

                StackPanelComponent.Children.Add(radioButton);
            }
        }

        private void OnClicked(object o, EventArgs e)
        {
            string value = StackPanelComponent.Children.OfType<RadioButton>().FirstOrDefault(rb => rb.IsChecked ?? false)?.Content?.ToString() ?? string.Empty;
            object enumValue = Enum.Parse(_objectLink.PropertyType(), value);
            
            _objectLink.Update(enumValue);
        }
    }
}