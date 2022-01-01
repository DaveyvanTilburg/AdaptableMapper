using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Xml;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Json;
using MappingFramework.Languages.DataStructure.Configuration;
using MappingFramework.MappingInterface.AvalonEdit;
using MappingFramework.MappingInterface.Controls;
using MappingFramework.MappingInterface.Storage;

namespace MappingFramework.MappingInterface
{
    public partial class MappingWindow : Window
    {
        public static ContentType SourceType;
        public static ContentType TargetType;
        
        private readonly MappingConfiguration _mappingConfiguration;
        private readonly Dictionary<string, IFoldingSet> _foldingSets;

        private readonly string _dataStructureCreatorSource;

        public MappingWindow(string name, MappingConfiguration mappingConfiguration, ContentType sourceType, ContentType targetType)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            _mappingConfiguration = mappingConfiguration;
            _foldingSets = new Dictionary<string, IFoldingSet>();

            SourceType = sourceType;
            TargetType = targetType;

            Initialized += Load;
            InitializeComponent();

            BackButton.Click += OnBackButtonClick;
            SaveButton.Click += OnSaveButtonClick;
            TestButton.Click += OnTestButtonClick;

            NameTextBox.Text = name;
        }

        public MappingWindow(string name, MappingConfiguration mappingConfiguration, ContentType sourceType, ContentType targetType, string dataStructureCreatorSource) 
            : this(name, mappingConfiguration, sourceType, targetType)
        {
            _dataStructureCreatorSource = dataStructureCreatorSource;

            object targetExample = new DataStructureTargetCreator().Create(new Context(null,null,null), _dataStructureCreatorSource);
            TargetTextBox.Text = JsonSerializer.Serialize(targetExample);
        }

        private void Load(object o, EventArgs e)
        {
            switch (SourceType)
            {
                case ContentType.Xml:
                    SourceTextBox.Text = GetResource("MappingFramework.MappingInterface.Examples.XmlSource.xml");
                    LoadSyntax(SourceTextBox, "xml.xshd");
                    break;
                case ContentType.Json:
                    SourceTextBox.Text = GetResource("MappingFramework.MappingInterface.Examples.JsonSource.json");
                    LoadSyntax(SourceTextBox, "json.xshd");
                    break;
            }

            _foldingSets.Add(SourceTextBox.Name, FoldingSet.Create(SourceTextBox, SourceType));
            _foldingSets[SourceTextBox.Name].Update();
            
            switch (TargetType)
            {
                case ContentType.Xml:
                    TargetTextBox.Text = GetResource("MappingFramework.MappingInterface.Examples.XmlTarget.xml");
                    LoadSyntax(TargetTextBox, "xml.xshd");
                    break;
                case ContentType.Json:
                    TargetTextBox.Text = GetResource("MappingFramework.MappingInterface.Examples.JsonTarget.json");
                    LoadSyntax(TargetTextBox, "json.xshd");
                    break;
                case ContentType.DataStructure:
                    LoadSyntax(TargetTextBox, "json.xshd");
                    break;
                case ContentType.Dictionary:
                    TargetTextBox.Visibility = Visibility.Hidden;
                    break;
            }

            _foldingSets.Add(TargetTextBox.Name, FoldingSet.Create(TargetTextBox, TargetType));
            _foldingSets[TargetTextBox.Name].Update();
            
            MainStackPanel.Children.Add(new ComponentControl(_mappingConfiguration, null));

            SourceTextBox.TextChanged += TextBoxOnTextChanged;
            TargetTextBox.TextChanged += TextBoxOnTextChanged;
        }

        private void TextBoxOnTextChanged(object sender, EventArgs e)
        {
            if (sender is not TextEditor textEditor)
                return;

            _foldingSets[textEditor.Name].Update();
        }

        private void LoadSyntax(TextEditor textEditor, string fileName)
        {
            using Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(LoadAssemblyFile(fileName));
            using XmlTextReader reader = new XmlTextReader(s);
            textEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
        }

        private string LoadAssemblyFile(string fileName)
            => Assembly.GetExecutingAssembly().GetManifestResourceNames().Single(f => f.EndsWith(fileName));

        private void OnTestButtonClick(object o, EventArgs e)
        {
            string source = SourceTextBox.Text;
            string target = TargetType == ContentType.DataStructure ? _dataStructureCreatorSource : TargetTextBox.Text;

            MapResult mapResult = _mappingConfiguration.Map(source, target);
            new ResultWindow(mapResult, TargetType).Show();
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