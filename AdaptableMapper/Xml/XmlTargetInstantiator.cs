using AdaptableMapper.Contexts;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public class XmlTargetInstantiator : TargetInstantiator
    {
        public XmlTargetInstantiator(string template)
        {
            Template = template;
        }
        public string Template { get; set; }

        public object Create()
        {
            XElement root = XElement.Parse(Template);
            RemoveAllNamespaces(root);

            return root;
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