using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MappingFramework.ContentTypes;
using MappingFramework.ContextTypes;
using MappingFramework.MappingInterface.Fields;
using MappingFramework.MappingInterface.Generics;
using MappingFramework.MappingInterface.Identifiers;

namespace MappingFramework.MappingInterface.Controls
{
    public partial class SelectionControl : UserControl
    {
        private readonly IObjectLink _objectLink;
        private readonly IIdentifierLink _identifierLink;

        public SelectionControl(IObjectLink objectLink, IIdentifierLink identifierLink)
        {
            _objectLink = objectLink;
            _identifierLink = identifierLink;

            Initialized += Load;
            InitializeComponent();

            SelectionComboBox.SelectionChanged += ComboBoxChanged;
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _objectLink.Name();

            List<string> options = OptionLists.List(_objectLink.PropertyType(), ContentType()).Select(t => t.Name).ToList();
            
            if (options.Count > 1)
                options.Insert(0, " ");

            foreach (string option in options)
                SelectionComboBox.Items.Add(option);

            object value = _objectLink.Value();

            if (value != null)
                LoadCurrentValue(value);
            else
            {
                SelectionComboBox.SelectedIndex = 0;
                ComboBoxChanged(null, null);
            }
        }

        private void ComboBoxChanged(object o, EventArgs e)
        {
            string selectedValue = SelectionComboBox.SelectedItem.ToString() ?? string.Empty;
            
            if(string.IsNullOrWhiteSpace(selectedValue))
            {
                _objectLink.Update(null);

                UnSubscribeAll();

                StackPanelComponent.Children.Clear();
            }
            else
            {
                Type valueType = OptionLists.List(_objectLink.PropertyType(), ContentType()).FirstOrDefault(t => t.Name.Equals(selectedValue, StringComparison.OrdinalIgnoreCase));
                object value = Activator.CreateInstance(valueType);
                _objectLink.Update(value);

                UnSubscribeAll();

                StackPanelComponent.Children.Clear();
                StackPanelComponent.Children.Add(new ComponentControl(value, _identifierLink));
            }
        }
        
        private void LoadCurrentValue(object value)
        {
            string valueTypeName = value.GetType().Name;

            foreach (string item in SelectionComboBox.Items)
                if (item.Equals(valueTypeName))
                    SelectionComboBox.SelectedIndex = SelectionComboBox.Items.IndexOf(item);

            StackPanelComponent.Children.Add(new ComponentControl(value, _identifierLink));
        }

        private void UnSubscribeAll()
        {
            IEnumerable<TextField> textFields = FindVisualChildren<TextField>(StackPanelComponent);

            foreach (TextField publisher in textFields)
                _identifierLink.UnSubscribe(publisher);
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child is T dependencyObject)
                        yield return dependencyObject;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }

        private ContentType ContentType()
        {
            ContextTypeAttribute attribute = (ContextTypeAttribute)_objectLink.PropertyType().GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(ContextTypeAttribute));
            ContextType contextType = attribute?.ContextType == ContextType.Target ? ContextType.Target : ContextType.Source;
            ContentType contentType = contextType == ContextType.Source ? MappingWindow.SourceType : MappingWindow.TargetType;

            return contentType;
        }
    }
}