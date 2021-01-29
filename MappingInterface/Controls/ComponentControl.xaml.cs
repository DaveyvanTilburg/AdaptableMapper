using System;
using System.Windows.Controls;
using MappingFramework.MappingInterface.Fields;
using MappingFramework.MappingInterface.Generics;
using MappingFramework.MappingInterface.Identifiers;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class ComponentControl : UserControl
    {
        private readonly object _subject;
        private readonly IIdentifierLink _identifierLink;

        public ComponentControl(object subject, IIdentifierLink identifierLink)
        {
            _subject = subject;
            _identifierLink = identifierLink;

            Initialized += Load;
            InitializeComponent();
        }

        private void Load(object o, EventArgs e)
        {
            var interfaceComponent = new ObjectComponent(_subject);

            foreach (IObjectLink objectLink in interfaceComponent.Links())
            {
                switch (objectLink.Type())
                {
                    case ObjectComponentDisplayType.TextBox:
                        StackPanelComponent.Children.Add(new TextField(objectLink, IdentifierLink()));
                        break;
                    case ObjectComponentDisplayType.NumberBox:
                        StackPanelComponent.Children.Add(new NumberField(objectLink));
                        break;
                    case ObjectComponentDisplayType.CheckBox:
                        StackPanelComponent.Children.Add(new CheckBoxField(objectLink));
                        break;
                    case ObjectComponentDisplayType.RadioGroupBox:
                        StackPanelComponent.Children.Add(new RadioGroupField(objectLink));
                        break;
                    case ObjectComponentDisplayType.CharBox:
                        StackPanelComponent.Children.Add(new CharField(objectLink));
                        break;
                    
                    case ObjectComponentDisplayType.Item:
                        StackPanelComponent.Children.Add(new SelectionControl(objectLink, IdentifierLink()));
                        break;
                    case ObjectComponentDisplayType.List:
                        StackPanelComponent.Children.Add(new ListOfTControl(objectLink));
                        break;
                    
                    case ObjectComponentDisplayType.Direct:
                        StackPanelComponent.Children.Add(new DirectComponentControl(objectLink.Value(), IdentifierLink()));
                        break;

                    case ObjectComponentDisplayType.Undefined:
                    default:
                        throw new Exception($"Type is not supported: {objectLink.PropertyType()}");
                }
            }
        }

        private IIdentifierLink IdentifierLink() => StackPanelComponent.Children.Count == 0 ? _identifierLink : new NullIdentifierLink();
    }
}