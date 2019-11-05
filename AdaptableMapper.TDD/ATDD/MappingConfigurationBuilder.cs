using System;
using System.Collections.Generic;

namespace AdaptableMapper.TDD.ATDD
{
    internal class MappingConfigurationBuilder
    {
        private MappingConfiguration _result;
        internal MappingConfigurationBuilder()
        {
            StartNew();
        }

        internal void StartNew()
        {
            _result = new MappingConfiguration(null, null, null);
        }

        internal MappingConfiguration GetResult()
        {
            MappingConfiguration tempResult = _result;
            StartNew();

            return tempResult;
        }

        internal void AddContextFactory()
        {
            _result.ContextFactory = new Contexts.ContextFactory(null, null);
        }

        internal void AddMappingScopeRoot()
        {
            _result.MappingScope = new MappingScopeRoot(new List<MappingScopeComposite>());
        }

        internal void AddNullObjectConverterForMappingConfiguration()
        {
            _result.ObjectConverter = new NullObjectConverter();
        }

        internal void AddModelObjectConverterToContextFactory()
        {
            _result.ContextFactory.ObjectConverter = new Model.ModelObjectConverter();
        }

        internal void AddXmlTargetInitiatorToContextFactory(string template)
        {
            _result.ContextFactory.TargetInstantiator = new Xml.XmlTargetInstantiator(template);
        }

        internal void AddXelementToStringObjectConverter()
        {
            _result.ObjectConverter = new Xml.XElementToStringObjectConverter();
        }
    }
}