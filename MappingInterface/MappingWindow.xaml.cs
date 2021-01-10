using System;
using System.Collections.Generic;
using System.Windows;
using MappingFramework.Configuration;
using MappingFramework.Configuration.DataStructure;
using MappingFramework.Configuration.Dictionary;
using MappingFramework.Configuration.Json;
using MappingFramework.Configuration.Xml;
using MappingFramework.MappingInterface.Controls;

namespace MappingFramework.MappingInterface
{
    public partial class MappingWindow : Window
    {
        private readonly ContentType _sourceType;
        private readonly ContentType _targetType;
        private readonly MappingConfiguration _mappingConfiguration;

        public MappingWindow(ContentType sourceType, ContentType targetType)
        {
            _sourceType = sourceType;
            _targetType = targetType;
            
            InitializeComponent();

            ObjectConverter objectConverter =
                sourceType == ContentType.Xml ? new XmlObjectConverter() :
                sourceType == ContentType.Json ? new JsonObjectConverter() :
                //sourceType == ContentType.DataStructure ? new DataStructureObjectConverter() :
                sourceType == ContentType.String ? new StringToDataStructureObjectConverter() : (ObjectConverter)null;

            TargetInstantiator targetInstantiator =
                targetType == ContentType.Xml ? new XmlTargetInstantiator() :
                targetType == ContentType.Json ? new JsonTargetInstantiator() :
                //sourceType == ContentType.DataStructure ? new DataStructureObjectConverter() :
                sourceType == ContentType.Dictionary ? new DictionaryTargetInstantiator() : (TargetInstantiator)null;

            ResultObjectConverter resultObjectConverter =
                targetType == ContentType.Xml ? new XElementToStringObjectConverter():
                targetType == ContentType.Json ? new JTokenToStringObjectConverter():
                //sourceType == ContentType.DataStructure ? new DataStructureToStringObjectConverter():
                sourceType == ContentType.Dictionary ? new DataStructureToStringObjectConverter(): (ResultObjectConverter)null;

            _mappingConfiguration = new MappingConfiguration(
                new List<MappingScopeComposite>(),
                new List<Mapping>(),
                new ContextFactory(objectConverter, targetInstantiator),
                resultObjectConverter);

            ObjectConverterPanel.Children.Add(new GenericControl(objectConverter));
            TargetInstantiatorPanel.Children.Add(new GenericControl(targetInstantiator));
            ResultObjectConverterPanel.Children.Add(new GenericControl(resultObjectConverter));

            ButtonTest.Click += OnTest;

            SourceTextBox.Text = @"<root><test/></root>";
            TargetTextBox.Text = @"<root><test/></root>";

            MappingListStackPanel.Children.Add(
                new ListOfTControl(
                        list => _mappingConfiguration.Mappings = (List<Mapping>)list,
                        typeof(List<Mapping>),
                        nameof(_mappingConfiguration.Mappings),
                        () => new Mapping(),
                        (updateAction, name, contentType, newItem) => new MappingControl((Mapping)newItem, _sourceType, _targetType),
                        ContentType.Undefined
                    )
                );
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