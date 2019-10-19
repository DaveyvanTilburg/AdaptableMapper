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
            string searchValue = source.GetXPathValues(_searchPath).First();

            if (string.IsNullOrWhiteSpace(searchValue))
                throw new Exception($"Path {_searchPath} gives so search results");

            string actualXPath = XPath.Replace("{{searchResult}}", searchValue);
            string value = source.GetXPathValues(actualXPath).First();

            target.SetPropertyValue(ObjectPath, value);
        }

        public override void Serialize(XElement target, Adaptable source)
        {
            throw new NotImplementedException();
        }
    }
}