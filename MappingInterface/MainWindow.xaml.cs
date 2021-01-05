using MappingFramework;
using System.Windows;

namespace MappingInterface
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
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
                ContentType.DataStructure;

            Mapping mapping = new Mapping(sourceType, targetType);
            mapping.Show();

            Close();
        }
    }
}