using System;
using System.IO;
using System.Reflection;
using System.Windows;
using MappingFramework.ContentTypes;
using MappingFramework.MappingInterface.Controls;
using MappingFramework.MappingInterface.Storage;

namespace MappingFramework.MappingInterface
{
    public partial class MappingWindow : Window
    {
        public static ContentType SourceType;
        public static ContentType TargetType;
        
        private readonly MappingConfiguration _mappingConfiguration;

        public MappingWindow(LoadFile loadFile)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _mappingConfiguration = loadFile.MappingConfiguration();

            SourceType = _mappingConfiguration.SourceType();
            TargetType = _mappingConfiguration.TargetType();

            Initialized += Load;
            InitializeComponent();

            BackButton.Click += OnBackButtonClick;
            SaveButton.Click += OnSaveButtonClick;
            TestButton.Click += OnTestButtonClick;

            NameTextBox.Text = loadFile.Name();
        }
        
        public MappingWindow(ContentType sourceType, ContentType targetType)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _mappingConfiguration = new MappingConfiguration();

            SourceType = sourceType;
            TargetType = targetType;

            Initialized += Load;
            InitializeComponent();

            BackButton.Click += OnBackButtonClick;
            SaveButton.Click += OnSaveButtonClick;
            TestButton.Click += OnTestButtonClick;

            NameTextBox.Text = "New mapping configuration";
        }
        
        private void Load(object o, EventArgs e)
        {
            string sourceText = string.Empty;
            switch (SourceType)
            {
                case ContentType.Xml:
                    sourceText = GetResource("MappingFramework.MappingInterface.Examples.XmlSource.xml");
                    break;
                case ContentType.Json:
                    sourceText = GetResource("MappingFramework.MappingInterface.Examples.JsonSource.json");
                    break;
            }

            string targetText = string.Empty;
            switch (TargetType)
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

            MainStackPanel.Children.Add(new ComponentControl(_mappingConfiguration, null));
        }
        
        private void OnTestButtonClick(object o, EventArgs e)
        {
            string source = SourceTextBox.Text;
            string target = TargetTextBox.Text;

            MapResult mapResult = _mappingConfiguration.Map(source, target);
            new ResultWindow(mapResult).Show();
        }
        
        private void OnSaveButtonClick(object o, EventArgs e)
        {
            SaveFile saveFile = new Saves().NewSaveFile(_mappingConfiguration, NameTextBox.Text);
            saveFile.Save();

            MessageBox.Show("Saved");
        }

        private string GetResource(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(name))
            using (StreamReader reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
        
        private void OnBackButtonClick(object o, EventArgs e)
        {
            MessageBoxResult result = ShowPrompt();

            if (result != MessageBoxResult.Yes)
                return;

            new SettingsWindow().Show();
            Close();
        }
        
        private MessageBoxResult ShowPrompt()
        {
            string message = "Are you sure you wish to leave this screen?";
            string caption = "Warning";

            MessageBoxButton buttonType = MessageBoxButton.YesNoCancel;
            MessageBoxImage imageType = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(message, caption, buttonType, imageType);
            return result;
        }
    }
}