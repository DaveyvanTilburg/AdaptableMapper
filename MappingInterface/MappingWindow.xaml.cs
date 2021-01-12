using System;
using System.Windows;
using MappingFramework.MappingInterface.Controls;

namespace MappingFramework.MappingInterface
{
    public partial class MappingWindow : Window
    {
        public static ContentType SourceType;
        public static ContentType TargetType;
        private readonly MappingConfiguration _mappingConfiguration;

        public MappingWindow(ContentType sourceType, ContentType targetType)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            SourceType = sourceType;
            TargetType = targetType;
            
            InitializeComponent();
            
            ButtonTest.Click += OnTest;

            SourceTextBox.Text = @"<root><tests><test>1</test><test>2</test></tests></root>";
            TargetTextBox.Text = @"<root><ids><id/></ids></root>";

            _mappingConfiguration = new MappingConfiguration();

            MainStackPanel.Children.Add(new ComponentControl(_mappingConfiguration, false));
        }
        
        private void OnTest(object o, EventArgs e)
        {
            string source = SourceTextBox.Text;
            string target = TargetTextBox.Text;

            string result = _mappingConfiguration.Map(source, target) as string;
            MessageBox.Show(result);
        }
    }
}