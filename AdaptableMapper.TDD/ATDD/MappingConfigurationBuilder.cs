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

        internal void AddTargetInitiatorToContextFactory(string type, params string[] parameters)
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

        internal void AddObjectConverterToContextFactory(string type)
        {
            switch (type.ToLower())
            {
                case "xml":
                    _result.ContextFactory.ObjectConverter = new Xml.XmlObjectConverter();
                    break;
                case "json":
                    _result.ContextFactory.ObjectConverter = new Json.JsonObjectConverter();
                    break;
                case "model":
                    _result.ContextFactory.ObjectConverter = new Model.ModelObjectConverter();
                    break;
            }
        }

        internal void AddObjectConverter(string type)
        {
            switch (type.ToLower())
            {
                case "xml":
                    _result.ObjectConverter = new Xml.XElementToStringObjectConverter();
                    break;
                case "json":
                    _result.ObjectConverter = new Json.JTokenToStringObjectConverter();
                    break;
                case "model":
                    _result.ObjectConverter = new Model.ModelToStringObjectConverter();
                    break;
                case "null":
                    _result.ObjectConverter = new NullObjectConverter();
                    break;
            }
        }
    }
}