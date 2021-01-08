using System;
using System.Windows.Controls;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Fields
{
    public partial class CheckBoxField : UserControl
    {
        private readonly InterfaceRequirement _interfaceRequirement;

        public CheckBoxField(InterfaceRequirement interfaceRequirement)
        {
            _interfaceRequirement = interfaceRequirement;

            Initialized += Load;
            InitializeComponent();

            ComponentCheckBox.Click += OnClicked;
        }

        private void Load(object o, EventArgs e)
        {
            ComponentLabel.Content = _interfaceRequirement.Name();
        }

        private void OnClicked(object o, EventArgs e)
        {
            _interfaceRequirement.Update(ComponentCheckBox.IsChecked ?? false);
        }
    }
}