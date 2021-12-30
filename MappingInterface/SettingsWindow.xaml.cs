using System;
using System.Windows;
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
                ContentType.DataStructure;

            ContentType targetType =
                TargetXml.IsChecked ?? false ? ContentType.Xml :
                TargetJson.IsChecked ?? false ? ContentType.Json :
                TargetDictionary.IsChecked ?? false ? ContentType.Dictionary :
                ContentType.DataStructure;

            MappingWindow mappingWindow = new MappingWindow("New mapping configuration", new MappingConfiguration(), sourceType, targetType);
            mappingWindow.Show();

            Close();
        }
        
        private void OnLoadButtonClick(object o, EventArgs e)
        {
            new LoadWindow().Show();
            Close();
        }
    }
}