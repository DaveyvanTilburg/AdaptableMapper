using System;
using System.Linq;
using System.Xml.Linq;

namespace XPathSerialization
{
    public class XPathSearch : XPathConfiguration
    {
        private string _searchPath;

        public XPathSearch(string xPath, string objectPath, string searchPath) : base(xPath, objectPath)
        {
            _searchPath = searchPath;
        }

        public override void DeSerialize(XElement source, Adaptable target)
        {
            string searchValue = null;
            if (!string.IsNullOrWhiteSpace(_searchPath))
                searchValue = source.GetXPathValues(_searchPath).First();

            string actualXPath = string.IsNullOrWhiteSpace(searchValue) ? XPath : XPath.Replace("{{searchResult}}", searchValue);
            string value = source.GetXPathValues(actualXPath).First();

            target.SetPropertyValue(ObjectPath, value);
        }

        public override void Serialize(XElement target, Adaptable source)
        {
            //string searchValue = null;
            //if (!string.IsNullOrWhiteSpace(_searchPath))
            //    searchValue = source.GetXPathValues(_searchPath).First();

            //string actualXPath = string.IsNullOrWhiteSpace(searchValue) ? XPath : XPath.Replace("{{searchResult}}", searchValue);
            string value = source.GetPropertyValue(ObjectPath);

            target.SetXPathValues(XPath, value);
        }
    }
}