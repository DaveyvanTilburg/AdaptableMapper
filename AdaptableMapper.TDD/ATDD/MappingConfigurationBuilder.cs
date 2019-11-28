using AdaptableMapper.Traversals;
using System;
using System.Collections.Generic;
using System.Linq;

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
            _result.ContextFactory = new Contexts.ContextFactory(null, null);
        }

        internal void AddTargetInitiatorToContextFactory(string type)
        {
            switch (type.ToLower())
            {
                case "xml":
                    _result.ContextFactory.TargetInstantiator = new Xml.XmlTargetInstantiator();
                    break;
                case "json":
                    _result.ContextFactory.TargetInstantiator = new Json.JsonTargetInstantiator();
                    break;
                case "model":
                    _result.ContextFactory.TargetInstantiator = new Model.ModelTargetInstantiator();
                    break;
            }
        }

        internal void AddObjectConverterToContextFactory(string type)
        {
            switch (type.ToLower())
            {
                case "xml":
                    _result.ContextFactory.ObjectConverter = new Xml.XmlObjectConverterRemovesNamespace();
                    break;
                case "json":
                    _result.ContextFactory.ObjectConverter = new Json.JsonObjectConverter();
                    break;
                case "modelbase":
                    _result.ContextFactory.ObjectConverter = new Model.ModelObjectConverter();
                    break;
                case "model":
                    Type itemType = typeof(ModelObjects.Simple.Item);
                    _result.ContextFactory.ObjectConverter = new Model.StringToModelObjectConverter(
                        new Model.ModelTargetInstantiatorSource
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
            var traversalToGetTemplate = CreateTraversalToGetTemplate(scopeCompositeModel.TraversalToGetTemplate);
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
                    return new Xml.XmlGetScope(scopeCompositeModel.GetScopeTraversalPath);
                case "json":
                    return new Json.JsonGetScope(scopeCompositeModel.GetScopeTraversalPath);
                case "model":
                    return new Model.ModelGetScope(scopeCompositeModel.GetScopeTraversalPath);
                default:
                    return null;
            }
        }

        private GetTemplateTraversal CreateGetTemplateTraversal(string type)
        {
            switch (type.ToLower())
            {
                case "xml":
                    return new Xml.XmlGetTemplate(string.Empty);
                case "json":
                    return new Json.JsonGetTemplateTraversal(string.Empty);
                case "model":
                    return new Model.ModelGetTemplate(string.Empty);
                default:
                    return null;
            }
        }

        private ChildCreator CreateChildCreator(string type)
        {
            switch (type.ToLower())
            {
                case "xml":
                    return new Xml.XmlChildCreator();
                case "json":
                    return new Json.JsonChildCreator();
                case "model":
                    return new Model.ModelChildCreator();
                default:
                    return null;
            }
        }

        internal void AddObjectConverter(string type)
        {
            switch (type.ToLower())
            {
                case "xml":
                    _result.ResultObjectConverter = new Xml.XElementToStringObjectConverter();
                    break;
                case "json":
                    _result.ResultObjectConverter = new Json.JTokenToStringObjectConverter();
                    break;
                case "model":
                    _result.ResultObjectConverter = new Model.ModelToStringObjectConverter();
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
                    return new Xml.XmlGetValue(string.Empty);
                case "json":
                    return new Json.JsonGetValue(string.Empty);
                case "model":
                    return new Model.ModelGetValue(string.Empty);
                default:
                    return null;
            }
        }

        private SetValueTraversal CreateSetValueTraversal(string type)
        {
            switch (type.ToLower())
            {
                case "xml":
                    return new Xml.XmlSetValue(string.Empty);
                case "json":
                    return new Json.JsonSetValue(string.Empty);
                case "model":
                    return new Model.ModelSetValueOnProperty(string.Empty);
                default:
                    return null;
            }
        }

        public void AddXmlMappingToRoot(MappingModel mappingModel)
        {
            _result.Mappings.Add(
                new Mapping(
                    new Xml.XmlGetValue(mappingModel.GetValueTraversal),
                    new Xml.XmlSetValue(mappingModel.SetValueTraversal)
                ));
        }

        public void AddEmptyScope()
        {
            _result.MappingScopeComposites.Add(new MappingScopeComposite(null, null, null, null, null));
        }
    }
}