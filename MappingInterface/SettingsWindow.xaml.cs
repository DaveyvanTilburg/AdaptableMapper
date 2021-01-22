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
        }

        private void Go_Click(object sender, RoutedEventArgs e)
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

            MappingWindow mappingWindow = new MappingWindow(sourceType, targetType);
            mappingWindow.Show();

            Close();
        }
    }
}