using System;
using System.Linq;
using System.Windows.Controls;
using MappingFramework.Conditions;
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
        
        public GenericControl(object subject, ContentType contentType)
        {
            _contentType = contentType;
            _subject = subject;

            Initialized += LoadObjectConverter;
            InitializeComponent();
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
                    case InterfaceRequirementType.NumberBox:
                        ComponentPanel.Children.Add(new NumberField(interfaceRequirement));
                        break;
                    case InterfaceRequirementType.CheckBox:
                        ComponentPanel.Children.Add(new CheckBoxField(interfaceRequirement));
                        break;
                    case InterfaceRequirementType.RadioGroupBox:
                        ComponentPanel.Children.Add(new RadioGroupField(interfaceRequirement));
                        break;
                    case InterfaceRequirementType.GetValueTraversal:
                        ComponentPanel.Children.Add(new GetValueTraversalControl(interfaceRequirement.UpdateAction(), interfaceRequirement.Name(), _contentType));
                        break;
                    case InterfaceRequirementType.SetValueTraversal:
                        ComponentPanel.Children.Add(new SetValueTraversalControl(interfaceRequirement.UpdateAction(), interfaceRequirement.Name(), _contentType));
                        break;
                    case InterfaceRequirementType.ValueMutation:
                        ComponentPanel.Children.Add(new ValueMutationControl(interfaceRequirement.UpdateAction(), interfaceRequirement.Name()));
                        break;
                    case InterfaceRequirementType.Condition:
                        ComponentPanel.Children.Add(new ConditionControl(interfaceRequirement.UpdateAction(), interfaceRequirement.Name(), _contentType));
                        break;
                    case InterfaceRequirementType.List:
                        ComponentPanel.Children.Add(
                            new ListOfTControl(
                                interfaceRequirement.UpdateAction(),
                                interfaceRequirement.PropertyType(),
                                interfaceRequirement.PropertyType().GenericTypeArguments.First().Name, 
                                (updateAction, name, contentType, newItem) => UserControl(interfaceRequirement.PropertyType().GenericTypeArguments.First(), updateAction, name, contentType), 
                                typeof(NullCondition), 
                                _contentType)
                            );
                        break;
                    //default:
                        //throw new Exception($"Type is not supported: {interfaceRequirement.PropertyType()}");
                }
            }
        }
        
        private UserControl UserControl(Type type, Action<object> updateAction, string name, ContentType contentType)
        {
            if (type == typeof(Condition))
                return new ConditionControl(updateAction, name, contentType);

            throw new Exception($"Type is not supported: {type}");
        }
    }
}