using System;
using System.Windows;
using MappingFramework.Configuration;
using MappingFramework.MappingInterface.Controls;

namespace MappingFramework.MappingInterface
{
    public partial class ResultWindow : Window
    {
        private readonly MapResult _mapResult;
        
        public ResultWindow(MapResult mapResult)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            _mapResult = mapResult;

            Initialized += Load;
            InitializeComponent();
        }
        
        private void Load(object o, EventArgs e)
        {
            TextBoxComponent.Text = _mapResult.Result as string ?? string.Empty;

            foreach (Information information in _mapResult.Information)
                InformationPanel.Children.Add(new InformationRowControl(information));
        }
    }
}