using System.Xml.Linq;

namespace XPathSerialization
{
    public static class XPathSerializer
    {
        public static void Deserialize(XPathConfiguration xPathConfiguration, string source, Adaptable target)
        {
            XElement root = XElement.Parse(source);
            RemoveAllNamespaces(root);

            xPathConfiguration.DeSerialize(root, target);
        }

        public static string Serialize(XPathConfiguration xPathConfiguration, string template, Adaptable source)
        {
            XElement target = XElement.Parse(template);
            RemoveAllNamespaces(target);

            xPathConfiguration.Serialize(target, source);

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