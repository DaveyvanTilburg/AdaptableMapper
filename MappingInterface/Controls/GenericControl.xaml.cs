using System;
using System.Linq;
using System.Windows.Controls;
using MappingFramework.Configuration;
using MappingFramework.MappingInterface.Fields;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class GenericControl : UserControl
    {
        private readonly object _subject;
        
        public GenericControl(object subject)
        {
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
                    case InterfaceRequirementType.CharBox:
                        ComponentPanel.Children.Add(new CharField(interfaceRequirement));
                        break;
                    
                    
                    case InterfaceRequirementType.Item:
                        ComponentPanel.Children.Add(new SelectionControl(interfaceRequirement.UpdateAction(), interfaceRequirement.Name(), interfaceRequirement.PropertyType()));
                        break;
                    
                    
                    case InterfaceRequirementType.List:
                        ComponentPanel.Children.Add(
                            new ListOfTControl(
                                interfaceRequirement.UpdateAction(),
                                interfaceRequirement.PropertyType(),
                                interfaceRequirement.PropertyType().GenericTypeArguments.First().Name,
                                (updateAction, name) => UserControl(interfaceRequirement.PropertyType().GenericTypeArguments.First(), updateAction, name)
                            )
                        );
                        break;
                    case InterfaceRequirementType.Direct:
                        var newValue = Activator.CreateInstance(interfaceRequirement.PropertyType());
                        interfaceRequirement.Update(newValue);
                        ComponentPanel.Children.Add(new GenericControl(newValue));
                        break;
                    
                    
                    default:
                        throw new Exception($"Type is not supported: {interfaceRequirement.PropertyType()}");
                }
            }
        }
        
        private UserControl UserControl(Type type, Action<object> updateAction, string name)
        {
            if (type == typeof(AdditionalSource))
            {
                var newValue = new AdditionalSourceList();
                updateAction(newValue);
                return new GenericControl(newValue);
            }
            
            if (type.IsInterface)
                return new SelectionControl(updateAction, name, type);
            
            if (type.IsClass)
            {
                var newValue = Activator.CreateInstance(type);
                updateAction(newValue);
                return new GenericControl(newValue);
            }

            throw new Exception($"Type is not supported: {type}");
        }
    }
}