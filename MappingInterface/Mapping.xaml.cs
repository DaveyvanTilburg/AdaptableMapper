using System;
using MappingFramework;
using System.Collections.Generic;
using System.Windows;
using MappingFramework.Configuration;
using MappingFramework.Configuration.DataStructure;
using MappingFramework.Configuration.Dictionary;
using MappingFramework.Configuration.Json;
using MappingFramework.Configuration.Xml;
using MappingFramework.MappingInterface;

namespace MappingInterface
{
    public partial class Mapping : Window
    {
        MappingConfiguration _mappingConfiguration;
        
        public Mapping(ContentType sourceType, ContentType targetType)
        {
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
                new List<MappingFramework.Configuration.Mapping>(),
                new ContextFactory(objectConverter, targetInstantiator),
                resultObjectConverter);

            MappingPanel.Children.Add(new GenericComponentView(objectConverter));
            MappingPanel.Children.Add(new GenericComponentView(targetInstantiator));
            MappingPanel.Children.Add(new GenericComponentView(resultObjectConverter));

            ButtonTest.Click += OnTest;
        }

        private void OnTest(object o, EventArgs e)
        {
            
        }
    }
}