using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XPathSerialization.XPathConfigurations
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

            ObjectPath path = ObjectPath.CreateObjectPath(AdaptablePath);

            Adaptable pathTarget = target.GetOrCreateAdaptable(new Queue<string>(path.Path));
            pathTarget.SetValue(path.PropertyName, value);
        }

        public override void Serialize(XElement target, Adaptable source)
        {
            string searchValue = null;
            if (!string.IsNullOrWhiteSpace(_searchPath))
            {
                var searchPath = new Stack<string>(_searchPath.Split('/'));
                string searchPropertyName = searchPath.Pop();

                Adaptable searchPathTarget = source.NavigateToAdaptable(new Queue<string>(searchPath));
                searchValue = searchPathTarget.GetValue(searchPropertyName);
            }

            string actualAdaptablePath = string.IsNullOrWhiteSpace(searchValue) ? AdaptablePath : AdaptablePath.Replace("{{searchResult}}", searchValue);
            ObjectPath path = ObjectPath.CreateObjectPath(actualAdaptablePath);

            Adaptable pathTarget = source.NavigateToAdaptable(new Queue<string>(path.Path));
            string value = pathTarget.GetValue(path.PropertyName);

            target.SetXPathValues(XPath, value);
        }
    }
}