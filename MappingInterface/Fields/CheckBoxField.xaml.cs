using System;
using System.Windows.Controls;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Fields
{
    public partial class CheckBoxField : UserControl
    {
        private readonly IObjectLink _objectLink;

        public CheckBoxField(IObjectLink objectLink)
        {
            _objectLink = objectLink;

            Initialized += Load;
            InitializeComponent();

            CheckBoxComponent.Click += OnClicked;
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _objectLink.Name();
            CheckBoxComponent.IsChecked = (bool)_objectLink.Value();
        }

        private void OnClicked(object o, EventArgs e)
        {
            _objectLink.Update(CheckBoxComponent.IsChecked ?? false);
        }
    }
}