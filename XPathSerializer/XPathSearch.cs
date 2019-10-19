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

        public override void DeSerialize(XElement root, Adaptable adaptable)
        {
            string searchValue = root.GetXPathValues(_searchPath).First();

            if (string.IsNullOrWhiteSpace(searchValue))
                throw new Exception($"Path {_searchPath} gives so search results");

            string actualXPath = XPath.Replace("{{searchResult}}", searchValue);
            string value = root.GetXPathValues(actualXPath).First();

            adaptable.SetPropertyValue(ObjectPath, value);
        }
    }
}