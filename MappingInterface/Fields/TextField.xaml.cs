using System;
using System.Windows.Controls;
using MappingFramework.MappingInterface.Generics;

namespace MappingFramework.MappingInterface.Fields
{
    partial class TextField : UserControl, Publisher<IdentifierLinkUpdateEventArgs>
    {
        private readonly ObjectComponentLink _objectComponentLink;

        public event EventHandler<IdentifierLinkUpdateEventArgs> UpdateEvent;

        public TextField(ObjectComponentLink objectComponentLink, IdentifierLink identifierLink)
        {
            _objectComponentLink = objectComponentLink;

            Initialized += Load;
            InitializeComponent();

            identifierLink?.SubscribeTo(this);
            ComponentTextBox.KeyUp += OnKeyUp;
        }

        private void Load(object o, EventArgs e)
        {
            ComponentLabel.Content = _objectComponentLink.Name();
        }

        private void OnKeyUp(object o, EventArgs e)
        {
            _objectComponentLink.Update(ComponentTextBox.Text);
            UpdateEvent?.Invoke(this, new IdentifierLinkUpdateEventArgs(ComponentTextBox.Text));
        }
    }
}