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
        private readonly Action<object> _updateAction;
        private readonly string _name;
        private readonly Type _type;
        private readonly IIdentifierLink _identifierLink;

        public SelectionControl(ObjectComponentLink objectComponentLink, IIdentifierLink identifierLink) 
            : this(objectComponentLink.UpdateAction(), objectComponentLink.Name(), objectComponentLink.PropertyType(), identifierLink) { }

        public SelectionControl(Action<object> updateAction, string name, Type type, IIdentifierLink identifierLink)
        {
            _updateAction = updateAction;
            _name = name;
            _type = type;
            _identifierLink = identifierLink;

            Initialized += Load;
            InitializeComponent();

            SelectionComboBox.SelectionChanged += ComboBoxChanged;
        }

        private void Load(object o, EventArgs e)
        {
            LabelComponent.Content = _name;

            List<string> options = OptionLists.List(_type, ContentType()).Select(t => t.Name).ToList();
            
            if (options.Count > 1)
                options.Insert(0, " ");

            foreach (string option in options)
                SelectionComboBox.Items.Add(option);

            SelectionComboBox.SelectedIndex = 0;
            ComboBoxChanged(null, null);
        }

        private void ComboBoxChanged(object o, EventArgs e)
        {
            string selectedValue = SelectionComboBox.SelectedItem.ToString() ?? string.Empty;
            
            if(string.IsNullOrWhiteSpace(selectedValue))
            {
                _updateAction(null);

                UnSubscribeAll();

                StackPanelComponent.Children.Clear();
            }
            else
            {
                Type valueType = OptionLists.List(_type, ContentType()).FirstOrDefault(t => t.Name.Equals(selectedValue, StringComparison.OrdinalIgnoreCase));
                object value = Activator.CreateInstance(valueType);
                _updateAction(value);

                UnSubscribeAll();

                StackPanelComponent.Children.Clear();
                StackPanelComponent.Children.Add(new ComponentControl(value, _identifierLink));
            }
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
            ContextTypeAttribute attribute = (ContextTypeAttribute)_type.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(ContextTypeAttribute));
            ContextType contextType = attribute?.ContextType == ContextType.Target ? ContextType.Target : ContextType.Source;
            ContentType contentType = contextType == ContextType.Source ? MappingWindow.SourceType : MappingWindow.TargetType;

            return contentType;
        }
    }
}