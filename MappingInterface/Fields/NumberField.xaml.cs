using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Fields
{
    public partial class NumberField : UserControl
    {
        private readonly IObjectLink _objectLink;

        public NumberField(IObjectLink objectLink)
        {
            _objectLink = objectLink;

            Initialized += Load;
            InitializeComponent();

            TextBoxComponent.LostFocus += OnFocusLost;
            TextBoxComponent.PreviewTextInput += NumericOnly;
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _objectLink.Name();
            TextBoxComponent.Text = _objectLink.Value() as string ?? string.Empty;
        }

        private void OnFocusLost(object o, EventArgs e)
        {
            int value = !string.IsNullOrWhiteSpace(TextBoxComponent.Text) ? int.Parse(TextBoxComponent.Text) : 0;
            _objectLink.Update(value);
        }

        private void NumericOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsNumeric(e.Text);
        }

        private static readonly Regex Regex = new Regex("^[0-9]+");
        private bool IsNumeric(string text) => !Regex.IsMatch(text);
    }
}