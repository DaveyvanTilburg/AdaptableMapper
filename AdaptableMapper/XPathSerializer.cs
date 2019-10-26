using Newtonsoft.Json;
using System.Xml.Linq;
using AdaptableMapper.XPathConfigurations;

namespace AdaptableMapper
{
    public static class XPathSerializer
    {
        public static void Serialize(XPathConfiguration data, string source, Adaptable target)
        {
            XElement root = XElement.Parse(source);
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

        private static void RemoveAllNamespaces(XElement element)
        {
            element.Name = element.Name.LocalName;

            foreach (var node in element.DescendantNodes())
                if (node is XElement xElement)
                    RemoveAllNamespaces(xElement);
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