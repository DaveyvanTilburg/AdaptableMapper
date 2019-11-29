using AdaptableMapper.Traversals;
using System;
using System.Collections.Generic;
using System.Linq;
using AdaptableMapper.Configuration;

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
            _result = new MappingConfiguration();
        }

        internal MappingConfiguration GetResult()
        {
            MappingConfiguration tempResult = _result;
            StartNew();

            return tempResult;
        }

        internal void AddContextFactory()
        {
            _result.ContextFactory = new ContextFactory(null, null);
        }

        internal void AddTargetInitiatorToContextFactory(string type)
        {
            switch (type.ToLower())
            {
                case "xml":
                    _result.ContextFactory.TargetInstantiator = new Configuration.Xml.XmlTargetInstantiator();
                    break;
                case "json":
                    _result.ContextFactory.TargetInstantiator = new Configuration.Json.JsonTargetInstantiator();
                    break;
                case "model":
                    _result.ContextFactory.TargetInstantiator = new Configuration.Model.ModelTargetInstantiator();
                    break;
            }
        }

        internal void AddObjectConverterToContextFactory(string type)
        {
            switch (type.ToLower())
            {
                case "xml":
                    _result.ContextFactory.ObjectConverter = new Configuration.Xml.XmlObjectConverter();
                    break;
                case "json":
                    _result.ContextFactory.ObjectConverter = new Configuration.Json.JsonObjectConverter();
                    break;
                case "modelbase":
                    _result.ContextFactory.ObjectConverter = new Configuration.Model.ModelObjectConverter();
                    break;
                case "model":
                    Type itemType = typeof(ModelObjects.Simple.Item);
                    _result.ContextFactory.ObjectConverter = new Configuration.Model.StringToModelObjectConverter(
                        new Configuration.Model.ModelTargetInstantiatorSource
                        {
                            AssemblyFullName = itemType.Assembly.FullName,
                            TypeFullName = itemType.FullName
                        }
                    );
                    break;
            }
        }

        internal void AddScopeToRoot(ScopeCompositeModel scopeCompositeModel)
        {
            var getScopeTraversal = CreateGetScopeTraversal(scopeCompositeModel);
            var traversalToGetTemplate = CreateGetTemplateTraversal(scopeCompositeModel.TraversalToGetTemplate);
            var childCreator = CreateChildCreator(scopeCompositeModel.ChildCreator);

            var scope = new MappingScopeComposite(
                new List<MappingScopeComposite>(),
                new List<Mapping>(),
                getScopeTraversal,
                traversalToGetTemplate,
                childCreator);

            _result.MappingScopeComposites.Add(scope);
        }

        private GetScopeTraversal CreateGetScopeTraversal(ScopeCompositeModel scopeCompositeModel)
        {
            switch (scopeCompositeModel.GetScopeTraversal.ToLower())
            {
                case "xml":
                    return new Traversals.Xml.XmlGetScopeTraversal(scopeCompositeModel.GetScopeTraversalPath);
                case "json":
                    return new Traversals.Json.JsonGetScopeTraversal(scopeCompositeModel.GetScopeTraversalPath);
                case "model":
                    return new Traversals.Model.ModelGetScopeTraversal(scopeCompositeModel.GetScopeTraversalPath);
                default:
                    return null;
            }
        }

        private GetTemplateTraversal CreateGetTemplateTraversal(string type)
        {
            switch (type.ToLower())
            {
                case "xml":
                    return new Traversals.Xml.XmlGetTemplateTraversal(string.Empty);
                case "json":
                    return new Traversals.Json.JsonGetTemplateTraversal(string.Empty);
                case "model":
                    return new Traversals.Model.ModelGetTemplateTraversal(string.Empty);
                default:
                    return null;
            }
        }

        private ChildCreator CreateChildCreator(string type)
        {
            switch (type.ToLower())
            {
                case "xml":
                    return new Configuration.Xml.XmlChildCreator();
                case "json":
                    return new Configuration.Json.JsonChildCreator();
                case "model":
                    return new Configuration.Model.ModelChildCreator();
                default:
                    return null;
            }
        }

        internal void AddObjectConverter(string type)
        {
            switch (type.ToLower())
            {
                case "xml":
                    _result.ResultObjectConverter = new Configuration.Xml.XElementToStringObjectConverter();
                    break;
                case "json":
                    _result.ResultObjectConverter = new Configuration.Json.JTokenToStringObjectConverter();
                    break;
                case "model":
                    _result.ResultObjectConverter = new Configuration.Model.ModelToStringObjectConverter();
                    break;
                case "null":
                    _result.ResultObjectConverter = new NullObjectConverter();
                    break;
            }
        }

        internal void AddMappingToLastScope(MappingModel mappingModel)
        {
            var getValueTraversal = CreateGetValueTraversal(mappingModel.GetValueTraversal);
            var setValueTraversal = CreateSetValueTraversal(mappingModel.SetValueTraversal);

            var mapping = new Mapping(
                getValueTraversal,
                setValueTraversal);

            var lastScope = _result.MappingScopeComposites.Last();

            lastScope.Mappings.Add(mapping);
        }

        private GetValueTraversal CreateGetValueTraversal(string type)
        {
            switch (type.ToLower())
            {
                case "xml":
                    return new Traversals.Xml.XmlGetValueTraversal(string.Empty);
                case "json":
                    return new Traversals.Json.JsonGetValueTraversal(string.Empty);
                case "model":
                    return new Traversals.Model.ModelGetValueTraversal(string.Empty);
                default:
                    return null;
            }
        }

        private SetValueTraversal CreateSetValueTraversal(string type)
        {
            switch (type.ToLower())
            {
                case "xml":
                    return new Traversals.Xml.XmlSetValueTraversal(string.Empty);
                case "json":
                    return new Traversals.Json.JsonSetValueTraversal(string.Empty);
                case "model":
                    return new Traversals.Model.ModelSetValueOnPropertyTraversal(string.Empty);
                default:
                    return null;
            }
        }

        public void AddXmlMappingToRoot(MappingModel mappingModel)
        {
            _result.Mappings.Add(
                new Mapping(
                    new Traversals.Xml.XmlGetValueTraversal(mappingModel.GetValueTraversal),
                    new Traversals.Xml.XmlSetValueTraversal(mappingModel.SetValueTraversal)
                ));
        }

        public void AddEmptyScope()
        {
            _result.MappingScopeComposites.Add(new MappingScopeComposite(null, null, null, null, null));
        }
    }
}