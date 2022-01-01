using System;
using System.Windows;
using System.Windows.Controls;
using MappingFramework.ContentTypes;

namespace MappingFramework.MappingInterface
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            InitializeComponent();

            GoButton.Click += OnGoButtonClick;
            LoadButton.Click += OnLoadButtonClick;
        }

        private void OnGoButtonClick(object sender, EventArgs e)
        {
            ContentType sourceType =
                SourceXml.IsChecked ?? false ? ContentType.Xml :
                SourceJson.IsChecked ?? false ? ContentType.Json :
                ContentType.Undefined;

            ContentType targetType =
                TargetXml.IsChecked ?? false ? ContentType.Xml :
                TargetJson.IsChecked ?? false ? ContentType.Json :
                TargetDictionary.IsChecked ?? false ? ContentType.Dictionary :
                TargetDataStructure.IsChecked ?? false ? ContentType.DataStructure :
                ContentType.Undefined;

            if (sourceType == ContentType.Undefined || targetType == ContentType.Undefined)
                return;

            if (targetType == ContentType.DataStructure)
            {
                MappingWindow mappingWindow = new MappingWindow("New mapping configuration", new MappingConfiguration(), sourceType, targetType, DataStructureSource.Text);
                mappingWindow.Show();
            }
            else
            {
                MappingWindow mappingWindow = new MappingWindow("New mapping configuration", new MappingConfiguration(), sourceType, targetType);
                mappingWindow.Show();
            }

            Close();
        }
        
        private void OnLoadButtonClick(object o, EventArgs e)
        {
            new LoadWindow().Show();
            Close();
        }

        private void Radio_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
                DataStructureSource.IsEnabled = radioButton.Name == "TargetDataStructure";
        }
    }
}