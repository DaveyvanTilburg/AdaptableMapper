using System;
using System.Windows.Controls;
using MappingFramework.MappingInterface.Generics;
using MappingFramework.MappingInterface.Identifiers;

namespace MappingFramework.MappingInterface.Fields
{
    partial class TextField : UserControl, Publisher<IdentifierLinkUpdateEventArgs>
    {
        private readonly IObjectLink _objectLink;

        public event EventHandler<IdentifierLinkUpdateEventArgs> UpdateEvent;

        public TextField(IObjectLink objectLink, IIdentifierLink identifierLink)
        {
            _objectLink = objectLink;

            Initialized += Load;
            InitializeComponent();

            identifierLink.SubscribeTo(this);
            TextboxComponent.KeyUp += OnKeyUp;
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _objectLink.Name();
            TextboxComponent.Text = _objectLink.Value() as string ?? string.Empty;

            UpdateEvent?.Invoke(this, new IdentifierLinkUpdateEventArgs(TextboxComponent.Text));
        }

        private void OnKeyUp(object o, EventArgs e)
        {
            _objectLink.Update(TextboxComponent.Text);
            UpdateEvent?.Invoke(this, new IdentifierLinkUpdateEventArgs(TextboxComponent.Text));
        }
    }
}