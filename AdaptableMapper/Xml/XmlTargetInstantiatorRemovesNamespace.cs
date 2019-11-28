using System;
using System.Xml.Linq;
using AdaptableMapper.Configuration;

namespace AdaptableMapper.Xml
{
    public sealed class XmlTargetInstantiatorRemovesNamespace : TargetInstantiator
    {
        public object Create(object source)
        {
            if (!(source is string template))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#32; Source is not of expected type string", "error", source, source?.GetType().Name);
                return new XElement("nullObject");
            }

            XElement root;
            try
            {
                root = XElement.Parse(template);
            }
            catch(Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("XML#33; Template is not valid Xml", "error", exception.GetType().Name, exception.Message);
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