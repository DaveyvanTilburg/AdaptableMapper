using System;
using System.Linq;
using System.Windows.Controls;
using MappingFramework.Configuration;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class ListOfTEntryControl : UserControl
    {
        private readonly IdentifierLink _identifierLink;
        private readonly ObjectComponentLink _objectComponentLink;
        private readonly ListOfTEntry _listOfTEntry;
        private readonly string _name;
        
        public ListOfTEntryControl(ObjectComponentLink objectComponentLink, ListOfTEntry listOfTEntry, string name)
        {
            _identifierLink = new IdentifierLink(UpdateLabel);
            _objectComponentLink = objectComponentLink;
            _listOfTEntry = listOfTEntry;
            _name = name;

            Initialized += Load;
            InitializeComponent();

            RemoveButton.Click += OnRemoveClick;
        }

        private void Load(object o, EventArgs e)
        {
            StackPanelComponent.Children.Add(UserControl());
            LabelComponent.Content = _name;
        }

        private UserControl UserControl()
        {
            Type type = _objectComponentLink.PropertyType().GetGenericArguments().First();

            if (type == typeof(AdditionalSource))
            {
                var newValue = new AdditionalSourceList();
                _listOfTEntry.Update(newValue);
                return new ComponentControl(newValue, false, _identifierLink);
            }

            if (type.IsInterface)
                return new SelectionControl(_listOfTEntry.Update, _objectComponentLink.PropertyType().GetGenericArguments().First().Name, type, _identifierLink);

            if (type.IsClass)
            {
                var newValue = Activator.CreateInstance(type);
                _listOfTEntry.Update(newValue);
                return new ComponentControl(newValue, false, _identifierLink);
            }

            throw new Exception($"Type is not supported: {type}");
        }

        private void UpdateLabel(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                LabelComponent.Content = _name;
            else
                LabelComponent.Content = $"{_name} - {text}";
        }

        private void OnRemoveClick(object o, EventArgs e)
        {
            _listOfTEntry.Remove();
            ((Panel)Parent).Children.Remove(this);
        }
    }
}