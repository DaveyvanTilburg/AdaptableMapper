using System;
using System.Windows.Controls;
using MappingFramework.MappingInterface.Identifiers;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class DirectComponentControl : UserControl
    {
        private readonly object _subject;
        private readonly IIdentifierLink _identifierLink;

        public DirectComponentControl(object subject, IIdentifierLink identifierLink)
        {
            _subject = subject;
            _identifierLink = identifierLink;

            Initialized += Load;
            InitializeComponent();
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _subject.GetType().Name;
            StackPanelComponent.Children.Add(new ComponentControl(_subject, _identifierLink));
        }
    }
}