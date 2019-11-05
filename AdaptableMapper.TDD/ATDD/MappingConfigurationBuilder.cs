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

        internal void AddJsonObjectConverterToContextFactory()
        {
            _result.ContextFactory.ObjectConverter = new Json.JsonObjectConverter();
        }

        internal void ModelTargetInitiatorToContextFactory(string assembly, string type)
        {
            _result.ContextFactory.TargetInstantiator = new Model.ModelTargetInstantiator(assembly, type);
        }

        internal void AddModelToStringObjectConverter()
        {
            _result.ObjectConverter = new Model.ModelToStringObjectConverter();
        }

        internal void AddXmlObjectConverterToContextFactory()
        {
            _result.ContextFactory.ObjectConverter = new Xml.XmlObjectConverter();
        }

        internal void AddJsonTargetInitiatorToContextFactory(string template)
        {
            _result.ContextFactory.TargetInstantiator = new Json.JsonTargetInstantiator(template);
        }

        internal void AddJTokenToStringObjectConverter()
        {
            _result.ObjectConverter = new Json.JTokenToStringObjectConverter();
        }

        internal void AddTargetInitiator(string type, params string[] parameters)
        {
            switch (type.ToLower())
            {
                case "xml":
                    _result.ContextFactory.TargetInstantiator = new Xml.XmlTargetInstantiator(parameters[0]);
                    break;
                case "json":
                    _result.ContextFactory.TargetInstantiator = new Json.JsonTargetInstantiator(parameters[0]);
                    break;
                case "model":
                    _result.ContextFactory.TargetInstantiator = new Model.ModelTargetInstantiator(parameters[0], parameters[1]);
                    break;
            }
        }
    }
}