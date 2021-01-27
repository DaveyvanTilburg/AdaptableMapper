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

            Initialized += LoadObjectConverter;
            InitializeComponent();
        }

        private void LoadObjectConverter(object o, EventArgs e)
        {
            var interfaceComponent = new ObjectComponent(_subject);

            foreach (IObjectLink objectLink in interfaceComponent.Links())
            {
                switch (objectLink.Type())
                {
                    case ObjectComponentDisplayType.TextBox:
                        ComponentPanel.Children.Add(new TextField(objectLink, IdentifierLink()));
                        break;
                    case ObjectComponentDisplayType.NumberBox:
                        ComponentPanel.Children.Add(new NumberField(objectLink));
                        break;
                    case ObjectComponentDisplayType.CheckBox:
                        ComponentPanel.Children.Add(new CheckBoxField(objectLink));
                        break;
                    case ObjectComponentDisplayType.RadioGroupBox:
                        ComponentPanel.Children.Add(new RadioGroupField(objectLink));
                        break;
                    case ObjectComponentDisplayType.CharBox:
                        ComponentPanel.Children.Add(new CharField(objectLink));
                        break;


                    case ObjectComponentDisplayType.Item:
                        ComponentPanel.Children.Add(new SelectionControl(objectLink, IdentifierLink()));
                        break;
                    case ObjectComponentDisplayType.List:
                        ComponentPanel.Children.Add(new ListOfTControl(objectLink));
                        break;
                    case ObjectComponentDisplayType.Direct:
                        if(objectLink.Value() == null)
                        {
                            var newValue = Activator.CreateInstance(objectLink.PropertyType());
                            objectLink.Update(newValue);
                        }
                        
                        ComponentPanel.Children.Add(new DirectComponentControl(objectLink.Value(), IdentifierLink()));
                        break;


                    default:
                        throw new Exception($"Type is not supported: {objectLink.PropertyType()}");
                }
            }
        }

        private IIdentifierLink IdentifierLink() => ComponentPanel.Children.Count == 0 ? _identifierLink : new NullIdentifierLink();
    }
}