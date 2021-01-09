using System;
using System.Windows.Controls;
using MappingFramework.Configuration;
using MappingFramework.Traversals;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class MappingControl : UserControl
    {
        private readonly Mapping _mapping;
        private readonly ContentType _sourceType;
        private readonly ContentType _targetType;

        public MappingControl(Mapping mapping, ContentType sourceType, ContentType targetType)
        {
            _mapping = mapping;
            _sourceType = sourceType;
            _targetType = targetType;

            Initialized += Load;
            InitializeComponent();
        }
        
        private void Load(object o, EventArgs e)
        {
            GetValueStackPanelComponent.Children.Add(
                new GetValueTraversalControl(getValueTraversal => _mapping.GetValueTraversal = (GetValueTraversal)getValueTraversal, nameof(_mapping.GetValueTraversal), _sourceType));
            
            SetValueStackPanelComponent.Children.Add(
                new SetValueTraversalControl(setValueTraversal => _mapping.SetValueTraversal = (SetValueTraversal)setValueTraversal, nameof(_mapping.SetValueTraversal), _targetType));
        }
    }
}