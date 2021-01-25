using System;
using System.Windows.Controls;
using MappingFramework.Process;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class InformationRowControl : UserControl
    {
        private readonly Information _information;
        
        public InformationRowControl(Information information)
        {
            _information = information;

            Initialized += Load;
            InitializeComponent();
        }
        
        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _information.Type.ToString();
            TextBoxComponent.Text = _information.Message;
        }
    }
}