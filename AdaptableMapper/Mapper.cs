using AdaptableMapper.Contexts;
using AdaptableMapper.Traversals;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace AdaptableMapper
{
    public static class Mapper
    {
        public static void Serialize(MappingConfiguration mappingConfiguration, object source, object targetInstantiationMaterial)
        {
            Context context = mappingConfiguration.ContextFactory.Create(source, targetInstantiationMaterial);

            mappingConfiguration.ScopeTraversalComposite.Traverse(context);

            
            RemoveAllNamespaces(root);

            XPathTransformation childConfiguration = XPathConfigurationRepository.GetInstance().GetConfiguration(data.Type);
            childConfiguration.Serialize(data, root, target);
        }

        public static string Deserialize(XPathConfiguration data, string template, Adaptable source)
        {
            XElement target = XElement.Parse(template);
            RemoveAllNamespaces(target);

            XPathTransformation childConfiguration = XPathConfigurationRepository.GetInstance().GetConfiguration(data.Type);
            childConfiguration.Deserialize(data, target, source);

            return target.ToString();
        }

        

        public static string GetMemento(XPathConfiguration xPathConfiguration)
        {
            Formatting indented = Formatting.Indented;
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            string serialized = JsonConvert.SerializeObject(xPathConfiguration, indented, settings);
            return serialized;
        }

        public static XPathConfiguration LoadMemento(string memento)
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
            var deserialized = JsonConvert.DeserializeObject<XPathConfiguration>(memento, settings);
            return deserialized;
        }
    }
}