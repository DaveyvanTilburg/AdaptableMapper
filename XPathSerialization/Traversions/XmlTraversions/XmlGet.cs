using System.Xml.Linq;

namespace XPathSerialization.Traversions.XmlTraversions
{
    public class XmlGet : GetTraversion
    {
        public string Path { get; set; }

        public string GetValue(object source)
        {
            if (!(source is XElement xElement))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type XElement");
                return string.Empty;
            }

            return xElement.GetXPathValue(Path);
        }
    }
}