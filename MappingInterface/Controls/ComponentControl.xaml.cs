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

            foreach (ObjectComponentLink objectComponentLink in interfaceComponent.Links())
            {
                switch (objectComponentLink.Type())
                {
                    case ObjectComponentDisplayType.TextBox:
                        ComponentPanel.Children.Add(new TextField(objectComponentLink, IdentifierLink()));
                        break;
                    case ObjectComponentDisplayType.NumberBox:
                        ComponentPanel.Children.Add(new NumberField(objectComponentLink));
                        break;
                    case ObjectComponentDisplayType.CheckBox:
                        ComponentPanel.Children.Add(new CheckBoxField(objectComponentLink));
                        break;
                    case ObjectComponentDisplayType.RadioGroupBox:
                        ComponentPanel.Children.Add(new RadioGroupField(objectComponentLink));
                        break;
                    case ObjectComponentDisplayType.CharBox:
                        ComponentPanel.Children.Add(new CharField(objectComponentLink));
                        break;


                    case ObjectComponentDisplayType.Item:
                        ComponentPanel.Children.Add(new SelectionControl(objectComponentLink.UpdateAction(), objectComponentLink.Name(), objectComponentLink.PropertyType(), IdentifierLink()));
                        break;
                    case ObjectComponentDisplayType.List:
                        ComponentPanel.Children.Add(new ListOfTControl(objectComponentLink));
                        break;
                    case ObjectComponentDisplayType.Direct:
                        var newValue = Activator.CreateInstance(objectComponentLink.PropertyType());
                        objectComponentLink.Update(newValue);
                        ComponentPanel.Children.Add(new DirectComponentControl(newValue, IdentifierLink()));
                        break;


                    default:
                        throw new Exception($"Type is not supported: {objectComponentLink.PropertyType()}");
                }
            }
        }

        private IIdentifierLink IdentifierLink() => ComponentPanel.Children.Count == 0 ? _identifierLink : new NullIdentifierLink();
    }
}