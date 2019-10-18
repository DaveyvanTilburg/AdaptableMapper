using System.Xml.Linq;

namespace XPathSerialization
{
    public static class XPathSerializer
    {
        public static void Adept(XPathConfiguration xPathConfiguration, string source, Adaptable adaptable)
        {
            XElement root = XElement.Parse(source);
            RemoveAllNamespaces(root);

            xPathConfiguration.DeSerialize(root, adaptable);
        }

        private static void RemoveAllNamespaces(XElement element)
        {
            element.Name = element.Name.LocalName;

            foreach (var node in element.DescendantNodes())
            {
                if (node is XElement xElement)
                    RemoveAllNamespaces(xElement);
            }
        }
    }
}