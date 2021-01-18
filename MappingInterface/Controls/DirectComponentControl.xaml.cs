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

            Initialized += LoadObjectConverter;
            InitializeComponent();
        }

        private void LoadObjectConverter(object o, EventArgs e)
        {
            LabelComponent.Content = _subject.GetType().Name;

            ComponentPanel.Children.Add(new ComponentControl(_subject, _identifierLink));
        }
    }
}