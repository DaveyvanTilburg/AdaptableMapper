using System;
using System.Linq;
using System.Windows.Controls;
using MappingFramework.Configuration;
using MappingFramework.MappingInterface.Generics;
using MappingFramework.MappingInterface.Identifiers;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class ListOfTEntryControl : UserControl
    {
        private readonly IIdentifierLink _identifierLink;
        private readonly IObjectLink _objectLink;
        private readonly ListOfTEntry _listOfTEntry;
        
        public ListOfTEntryControl(IObjectLink objectLink, ListOfTEntry listOfTEntry)
        {
            _identifierLink = new IdentifierLink(UpdateLabel);
            _objectLink = objectLink;
            _listOfTEntry = listOfTEntry;

            Initialized += Load;
            InitializeComponent();

            RemoveButton.Click += OnRemoveClick;
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = ComponentName();
            StackPanelComponent.Children.Add(UserControl());
        }

        private UserControl UserControl()
        {
            Type type = _objectLink.PropertyType().GetGenericArguments().First();

            if (type == typeof(AdditionalSource))
            {
                object value = _listOfTEntry.Value();
                
                if(value == null)
                {
                    value = new AdditionalSourceList();
                    _listOfTEntry.Update(value);
                }
                
                return new ComponentControl(value, _identifierLink);
            }

            if (type.IsInterface)
                return new SelectionControl(new ListItemLink(_listOfTEntry.Update, _listOfTEntry.Value, ComponentName(), type), _identifierLink);

            if (type.IsClass)
            {
                object value = _listOfTEntry.Value();

                if (value == null)
                {
                    value = Activator.CreateInstance(type);
                    _listOfTEntry.Update(value);
                }

                return new ComponentControl(value, _identifierLink);
            }

            throw new Exception($"Type is not supported: {type}");
        }

        private void UpdateLabel(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                LabelComponent.Content = ComponentName();
            else
                LabelComponent.Content = $"{ComponentName()} - {text}";
        }

        private void OnRemoveClick(object o, EventArgs e)
        {
            _listOfTEntry.Remove();
            ((Panel)Parent).Children.Remove(this);
        }
        
        private string ComponentName() => _objectLink.PropertyType().GetGenericArguments().First().Name;
    }
}