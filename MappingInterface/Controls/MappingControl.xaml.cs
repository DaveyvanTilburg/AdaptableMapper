using System;
using System.Windows.Controls;
using MappingFramework.Configuration;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class MappingControl : UserControl
    {
        private readonly Mapping _mapping;

        public MappingControl(Mapping mapping)
        {
            _mapping = mapping;

            Initialized += Load;
            InitializeComponent();
        }
        
        private void Load(object o, EventArgs e)
        {
            var getValueInterfaceRequirement = new InterfaceRequirement(_mapping.GetType().GetProperty(nameof(_mapping.GetValueTraversal)), _mapping);
            GetValueStackPanelComponent.Children.Add(
                new SelectionControl(
                    getValueInterfaceRequirement.UpdateAction(), 
                    getValueInterfaceRequirement.Name(), 
                    getValueInterfaceRequirement.PropertyType())
                );

            var setValueInterfaceRequirement = new InterfaceRequirement(_mapping.GetType().GetProperty(nameof(_mapping.SetValueTraversal)), _mapping);
            SetValueStackPanelComponent.Children.Add(
                new SelectionControl(
                    setValueInterfaceRequirement.UpdateAction(), 
                    setValueInterfaceRequirement.Name(), 
                    setValueInterfaceRequirement.PropertyType())
                );
        }
    }
}