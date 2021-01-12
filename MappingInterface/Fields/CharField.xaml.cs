using System;
using System.Linq;
using System.Windows.Controls;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Fields
{
    partial class CharField : UserControl
    {
        private readonly ObjectComponentLink _objectComponentLink;
        
        public CharField(ObjectComponentLink objectComponentLink)
        {
            _objectComponentLink = objectComponentLink;

            Initialized += Load;
            InitializeComponent();

            ComponentTextBox.LostFocus += OnFocusLost;
        }

        private void Load(object o, EventArgs e)
        {
            ComponentLabel.Content = _objectComponentLink.Name();
        }

        private void OnFocusLost(object o, EventArgs e)
        {
            _objectComponentLink.Update(ComponentTextBox.Text.FirstOrDefault());
        }
    }
}