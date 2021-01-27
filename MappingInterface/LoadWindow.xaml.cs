using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MappingFramework.Languages.DataStructure.Configuration;
using MappingFramework.MappingInterface.Storage;

namespace MappingFramework.MappingInterface
{
    public partial class LoadWindow : Window
    {
        public LoadWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            Initialized += Load;
            InitializeComponent();

            BackButton.Click += OnBackButtonClick;
            DeleteButton.Click += OnDeleteButtonClick;
            LoadButton.Click += OnLoadButtonClick;
        }

        private void Load(object o, EventArgs e)
            => LoadFiles();

        private void LoadFiles()
        {
            List<LoadFile> loadFiles = new Saves().LoadFiles();
            foreach (LoadFile loadFile in loadFiles)
                FileListComponent.Children.Add(new RadioButton
                {
                    GroupName = "LoadFile",
                    Content = loadFile.Name()
                });
        }
        
        private void OnBackButtonClick(object o, EventArgs e)
        {
            new SettingsWindow().Show();
            Close();
        }
        
        private void OnDeleteButtonClick(object o, EventArgs e)
        {
            new Saves().Load(FileName()).Delete();

            FileListComponent.Children.Clear();
            LoadFiles();
        }

        private void OnLoadButtonClick(object o, EventArgs e)
        {
            LoadFile loadFile = new Saves().Load(FileName());

            new MappingWindow(loadFile).Show();
            Close();
        }
        
        private string FileName()
            => FileListComponent.Children.OfType<RadioButton>().FirstOrDefault(rb => rb.IsChecked ?? false)?.Content?.ToString() ?? string.Empty;
    }
}