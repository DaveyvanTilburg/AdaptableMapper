using System;
using System.Windows.Controls;

namespace MappingFramework.MappingInterface
{
    public partial class CheckBoxField : UserControl
    {
        private readonly InterfaceRequirement _interfaceRequirement;

        public CheckBoxField(InterfaceRequirement interfaceRequirement)
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