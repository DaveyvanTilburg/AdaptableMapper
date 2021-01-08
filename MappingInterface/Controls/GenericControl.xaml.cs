using System;
using System.Windows.Controls;
using MappingFramework.MappingInterface.Fields;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class GenericControl : UserControl
    {
        private readonly object _subject;
        private readonly ContentType _contentType;
        
        public GenericControl(object subject)
        {
            _subject = subject;

            Initialized += LoadObjectConverter;
            InitializeComponent();
        }
        
        public GenericControl(object subject, ContentType contentType) : this(subject)
        {
            _contentType = contentType;
        }
        
        private void LoadObjectConverter(object o, EventArgs e)
        {
            var interfaceComponent = new InterfaceComponent(_subject);

            foreach(InterfaceRequirement interfaceRequirement in interfaceComponent.Requirements())
            {
                switch(interfaceRequirement.Type())
                {
                    case InterfaceRequirementType.TextBox:
                        ComponentPanel.Children.Add(new TextField(interfaceRequirement));
                        break;
                    case InterfaceRequirementType.CheckBox:
                        ComponentPanel.Children.Add(new CheckBoxField(interfaceRequirement));
                        break;
                    case InterfaceRequirementType.RadioGroupBox:
                        ComponentPanel.Children.Add(new RadioGroupField(interfaceRequirement));
                        break;
                    case InterfaceRequirementType.GetValueTraversal:
                        ComponentPanel.Children.Add(new GetValueTraversalControl(interfaceRequirement.PropertyInfo(), _subject, _contentType));
                        break;
                    case InterfaceRequirementType.SetValueTraversal:
                        ComponentPanel.Children.Add(new SetValueTraversalControl(interfaceRequirement.PropertyInfo(), _subject, _contentType));
                        break;
                    //default:
                        //throw new Exception($"Type is not supported: {interfaceRequirement.PropertyType()}");
                }
            }
        }
    }
}