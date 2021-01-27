using System;
using System.Linq;
using System.Windows.Controls;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Fields
{
    partial class CharField : UserControl
    {
        private readonly IObjectLink _objectLink;
        
        public CharField(IObjectLink objectLink)
        {
            _objectLink = objectLink;

            Initialized += Load;
            InitializeComponent();

            TextBoxComponent.LostFocus += OnFocusLost;
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _objectLink.Name();
            TextBoxComponent.Text = ((char)_objectLink.Value()).ToString();
        }

        private void OnFocusLost(object o, EventArgs e)
        {
            _objectLink.Update(TextBoxComponent.Text.FirstOrDefault());
        }
    }
}