using System;
using System.Windows.Controls;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Fields
{
    public partial class CheckBoxField : UserControl
    {
        private readonly ObjectComponentLink _objectComponentLink;

        public CheckBoxField(ObjectComponentLink objectComponentLink)
        {
            _objectComponentLink = objectComponentLink;

            Initialized += Load;
            InitializeComponent();

            ComponentCheckBox.Click += OnClicked;
        }

        private void Load(object o, EventArgs e)
        {
            ComponentLabel.Content = _objectComponentLink.Name();
        }

        private void OnClicked(object o, EventArgs e)
        {
            _objectComponentLink.Update(ComponentCheckBox.IsChecked ?? false);
        }
    }
}