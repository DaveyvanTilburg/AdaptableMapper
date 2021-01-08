using System;
using System.Collections.Generic;
using System.Linq;
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
            IEnumerable<string> getValueTraversalOptions = OptionLists.GetValueTraversals(_sourceType).Select(t => t.GetType().Name);

            foreach (string option in getValueTraversalOptions)
                GetValueTraversalComboBox.Items.Add(option);

            IEnumerable<string> setValueTraversalOptions = OptionLists.SetValueTraversals(_targetType).Select(t => t.GetType().Name);

            foreach (string option in setValueTraversalOptions)
                SetValueTraversalComboBox.Items.Add(option);
        }

        private void OnRemoveClick(object o, EventArgs e)
        {
            _mappingList.Remove(_mapping);
            ((Panel)Parent).Children.Remove(this);
        }
    }
}