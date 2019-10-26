using System.Linq;
using System.Xml.Linq;

namespace XPathSerialization.Traversals.XmlTraversals
{
    public class XmlGetSearch : GetTraversal
    {
        public string Path { get; set; }
        public string SearchPath { get; set; }

        public string GetValue(object source)
        {
            if (!(source is XElement xElement))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type XElement");
                return string.Empty;
            }

            string searchValue = null;
            if (!string.IsNullOrWhiteSpace(SearchPath))
                searchValue = xElement.GetXPathValues(SearchPath).First();

            string actualXPath = string.IsNullOrWhiteSpace(searchValue) ? Path : Path.Replace("{{searchResult}}", searchValue);
            string value = xElement.GetXPathValues(actualXPath).First();

            return value;
        }
    }
}