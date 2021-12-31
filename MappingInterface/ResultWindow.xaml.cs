using System;
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
using MappingFramework.MappingInterface.AvalonEdit;
using MappingFramework.MappingInterface.Controls;

namespace MappingFramework.MappingInterface
{
    public partial class ResultWindow : Window
    {
        private readonly MapResult _mapResult;
        private readonly ContentType _contentType;
        
        public ResultWindow(MapResult mapResult, ContentType contentType)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            _mapResult = mapResult;
            _contentType = contentType;

            Initialized += Load;
            InitializeComponent();
        }
        
        private void Load(object o, EventArgs e)
        {
            TextBoxComponent.Text = _mapResult.Result as string ?? string.Empty;

            switch (_contentType)
            {
                case ContentType.Xml:
                    LoadSyntax(TextBoxComponent, "xml.xshd");
                    break;
                case ContentType.Json:
                case ContentType.Dictionary:
                    LoadSyntax(TextBoxComponent, "json.xshd");
                    break;
            }

            FoldingSet.Create(TextBoxComponent, _contentType).Update();

            foreach (Information information in _mapResult.Information)
                InformationPanel.Children.Add(new InformationRowControl(information));
        }

        private void LoadSyntax(TextEditor textEditor, string fileName)
        {
            using Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(LoadAssemblyFile(fileName));
            using XmlTextReader reader = new XmlTextReader(s);
            textEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
        }

        private string LoadAssemblyFile(string fileName)
            => Assembly.GetExecutingAssembly().GetManifestResourceNames().Single(f => f.EndsWith(fileName));
    }
}