using System;
using System.IO;
using System.Reflection;
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

            string sourceText = string.Empty;
            switch(sourceType)
            {
                case ContentType.Xml:
                    sourceText = GetResource("MappingFramework.MappingInterface.Examples.XmlSource.xml");
                    break;
                case ContentType.Json:
                    sourceText = GetResource("MappingFramework.MappingInterface.Examples.JsonSource.json");
                    break;
            }

            string targetText = string.Empty;
            switch (targetType)
            {
                case ContentType.Xml:
                    targetText = GetResource("MappingFramework.MappingInterface.Examples.XmlTarget.xml");
                    break;
                case ContentType.Json:
                    targetText = GetResource("MappingFramework.MappingInterface.Examples.JsonTarget.json");
                    break;
            }

            SourceTextBox.Text = sourceText;
            TargetTextBox.Text = targetText;

            _mappingConfiguration = new MappingConfiguration();

            MainStackPanel.Children.Add(new ComponentControl(_mappingConfiguration, false, null));
        }
        
        private void OnTest(object o, EventArgs e)
        {
            string source = SourceTextBox.Text;
            string target = TargetTextBox.Text;

            string result = _mappingConfiguration.Map(source, target) as string;
            MessageBox.Show(result);
        }
        
        private string GetResource(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(name))
            using (StreamReader reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}