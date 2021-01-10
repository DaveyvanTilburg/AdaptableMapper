using System;
using System.Linq;
using System.Windows.Controls;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Fields
{
    partial class CharField : UserControl
    {
        private readonly InterfaceRequirement _interfaceRequirement;
        
        public CharField(InterfaceRequirement interfaceRequirement)
        {
            _interfaceRequirement = interfaceRequirement;

            Initialized += Load;
            InitializeComponent();

            ComponentTextBox.LostFocus += OnFocusLost;
        }

        private void Load(object o, EventArgs e)
        {
            ComponentLabel.Content = _interfaceRequirement.Name();
        }

        private void OnFocusLost(object o, EventArgs e)
        {
            _interfaceRequirement.Update(ComponentTextBox.Text.FirstOrDefault());
        }
    }
}