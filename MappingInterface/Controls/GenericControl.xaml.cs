using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MappingFramework.MappingInterface.Fields;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class GenericControl : UserControl
    {
        private readonly object _subject;
        private readonly bool _showName;
        
        public GenericControl(object subject, bool showName)
        {
            _subject = subject;
            _showName = showName;

            Initialized += LoadObjectConverter;
            InitializeComponent();
        }
        
        private void LoadObjectConverter(object o, EventArgs e)
        {
            if (_showName)
                ComponentPanel.Children.Add(new Label { Content = _subject.GetType().Name, FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Colors.Red) });
            
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
                        ComponentPanel.Children.Add(new ListOfTControl(interfaceRequirement));
                        break;
                    case InterfaceRequirementType.Direct:
                        var newValue = Activator.CreateInstance(interfaceRequirement.PropertyType());
                        interfaceRequirement.Update(newValue);
                        ComponentPanel.Children.Add(new GenericControl(newValue, true));
                        break;
                    
                    
                    default:
                        throw new Exception($"Type is not supported: {interfaceRequirement.PropertyType()}");
                }
            }
        }
    }
}