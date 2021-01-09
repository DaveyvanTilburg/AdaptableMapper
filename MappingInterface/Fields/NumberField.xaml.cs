using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Fields
{
    public partial class NumberField : UserControl
    {
        private readonly InterfaceRequirement _interfaceRequirement;

        public NumberField(InterfaceRequirement interfaceRequirement)
        {
            _interfaceRequirement = interfaceRequirement;

            Initialized += Load;
            InitializeComponent();

            ComponentTextBox.LostFocus += OnFocusLost;
            ComponentTextBox.PreviewTextInput += NumericOnly;
        }

        private void Load(object o, EventArgs e)
        {
            ComponentLabel.Content = _interfaceRequirement.Name();
        }

        private void OnFocusLost(object o, EventArgs e)
        {
            int value = !string.IsNullOrWhiteSpace(ComponentTextBox.Text) ? int.Parse(ComponentTextBox.Text) : 0;
            _interfaceRequirement.Update(value);
        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsNumeric(e.Text);
        }

        private static readonly Regex Regex = new Regex("^[0-9]+");
        private bool IsNumeric(string text) => !Regex.IsMatch(text);
    }
}