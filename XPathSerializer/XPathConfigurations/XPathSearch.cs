using System.Linq;
using System.Xml.Linq;

namespace XPathSerialization.XPathConfigurations
{
    internal partial class XPathSearch : XPathConfiguration
    {
        private readonly string _searchPath;

        public XPathSearch(string xPath, string adaptableScope, string searchPath) : base(xPath, adaptableScope)
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

            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(AdaptablePath);

            Adaptable pathTarget = target.GetOrCreateAdaptable(adaptablePathContainer.GetPath());
            pathTarget.SetValue(adaptablePathContainer.PropertyName, value);
        }

        public override void Serialize(XElement target, Adaptable source)
        {
            string searchValue = null;
            if (!string.IsNullOrWhiteSpace(_searchPath))
            {
                var searchAdaptablePath = AdaptablePathContainer.CreateAdaptablePath(_searchPath);

                Adaptable searchPathTarget = source.NavigateToAdaptable(searchAdaptablePath.GetPath());
                searchValue = searchPathTarget.GetValue(searchAdaptablePath.PropertyName);
            }

            string actualAdaptablePath = string.IsNullOrWhiteSpace(searchValue) ? AdaptablePath : AdaptablePath.Replace("{{searchResult}}", searchValue);
            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(actualAdaptablePath);

            Adaptable pathTarget = source.NavigateToAdaptable(adaptablePathContainer.GetPath());
            string value = pathTarget.GetValue(adaptablePathContainer.PropertyName);

            target.SetXPathValues(XPath, value);
        }
    }
}