using System.Xml.Linq;
using XPathSerialization.XPathConfigurations;

namespace XPathSerialization
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
    }
}