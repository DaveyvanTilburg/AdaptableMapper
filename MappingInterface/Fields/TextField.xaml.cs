using System;
using System.Windows.Controls;

namespace MappingFramework.MappingInterface
{
    partial class TextField : UserControl
    {
        private readonly InterfaceRequirement _interfaceRequirement;
        
        public TextField(InterfaceRequirement interfaceRequirement)
        {
            _interfaceRequirement = interfaceRequirement;

            Initialized += Load;
            InitializeComponent();
        }

        private void Load(object o, EventArgs e)
        {
            ComponentLabel.Content = _interfaceRequirement.Name();
        }
    }
}