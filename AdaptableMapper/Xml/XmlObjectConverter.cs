using System;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlObjectConverter : ObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is string input))
            {
                Errors.ErrorObservable.GetInstance().Raise("XML#18; source is not of expected type String", source);
                return string.Empty;
            }

            XElement root;
            try
            {
                root = XElement.Parse(input);
            }
            catch(Exception exception)
            {
                Errors.ErrorObservable.GetInstance().Raise("XML#19; input could not be parsed to XElement", input, exception);
                root = new XElement("nullObject");
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