using AdaptableMapper.Contexts;
using System;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlTargetInstantiator : TargetInstantiator
    {
        public XmlTargetInstantiator(string template)
        {
            Template = template;
        }
        public string Template { get; set; }

        public object Create()
        {
            XElement root;
            try
            {
                root = XElement.Parse(Template);
            }
            catch(Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("XML#6; Template is not valid Xml", "error", exception.GetType().Name, exception.Message);
                return new XElement("nullObject");
            }
            
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