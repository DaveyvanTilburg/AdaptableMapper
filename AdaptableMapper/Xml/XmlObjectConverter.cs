using AdaptableMapper.Contexts;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public class XmlObjectConverter : ObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is string input))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type String");
                return string.Empty;
            }

            XElement root = XElement.Parse(input);
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