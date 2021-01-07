﻿using System;
using System.Windows.Controls;
using MappingFramework.MappingInterface.Fields;

namespace MappingFramework.MappingInterface
{
    public partial class GenericComponentView : UserControl
    {
        private readonly object _subject;
        
        public GenericComponentView(object subject)
        {
            _subject = subject;

            Initialized += LoadObjectConverter;
            InitializeComponent();
        }
        
        private void LoadObjectConverter(object o, EventArgs e)
        {
            var interfaceComponent = new InterfaceComponent(_subject);

            foreach(InterfaceRequirement interfaceRequirement in interfaceComponent.Requirements())
            {
                switch(interfaceRequirement.Type())
                {
                    case InterfaceRequirementType.TextBox:
                        ComponentPanel.Children.Add(new TextField(interfaceRequirement));
                        break;
                    case InterfaceRequirementType.CheckBox:
                        ComponentPanel.Children.Add(new CheckBoxField(interfaceRequirement));
                        break;
                    case InterfaceRequirementType.RadioGroupBox:
                        ComponentPanel.Children.Add(new RadioGroupField(interfaceRequirement));
                        break;
                }
            }
        }
    }
}