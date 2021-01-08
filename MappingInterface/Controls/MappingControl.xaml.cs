using System;
using System.Collections.Generic;
using System.Windows.Controls;
using MappingFramework.Configuration;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class MappingControl : UserControl
    {
        private readonly Mapping _mapping;
        private readonly List<Mapping> _mappingList;
        private readonly ContentType _sourceType;
        private readonly ContentType _targetType;

        public MappingControl(Mapping mapping, List<Mapping> mappingList, ContentType sourceType, ContentType targetType)
        {
            _mapping = mapping;
            _mappingList = mappingList;
            _sourceType = sourceType;
            _targetType = targetType;

            Initialized += Load;
            InitializeComponent();
            
            RemoveButton.Click += OnRemoveClick;
        }
        
        private void Load(object o, EventArgs e)
        {
            GetValueStackPanelComponent.Children.Add(new GetValueTraversalControl(_mapping.GetType().GetProperty(nameof(_mapping.GetValueTraversal)), _mapping, _sourceType));
            SetValueStackPanelComponent.Children.Add(new SetValueTraversalControl(_mapping.GetType().GetProperty(nameof(_mapping.SetValueTraversal)), _mapping, _targetType));
        }

        private void OnRemoveClick(object o, EventArgs e)
        {
            _mappingList.Remove(_mapping);
            ((Panel)Parent).Children.Remove(this);
        }
    }
}