using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlObjectConverter : ObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is string input))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type String");
                return string.Empty;
            }

            XElement root = XElement.Parse(input);
            if (root == null)
            {
                Errors.ErrorObservable.GetInstance().Raise("Source could not be parsed to XElement");
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