using System.Linq;
using System.Xml.Linq;

namespace AdaptableMapper.XPathConfigurations
{
    internal class XPathSearch : XPathTransformation
    {
        public void Serialize(XPathConfiguration configuration, XElement source, Adaptable target)
        {
            string searchValue = null;
            if (!string.IsNullOrWhiteSpace(configuration.SearchPath))
                searchValue = source.GetXPathValues(configuration.SearchPath).First();

            string actualXPath = string.IsNullOrWhiteSpace(searchValue) ? configuration.XPath : configuration.XPath.Replace("{{searchResult}}", searchValue);
            string value = source.GetXPathValues(actualXPath).First();

            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(configuration.AdaptablePath);

            Adaptable pathTarget = target.GetOrCreateAdaptable(adaptablePathContainer.CreatePathQueue());
            pathTarget.SetValue(adaptablePathContainer.PropertyName, value);
        }

        public void Deserialize(XPathConfiguration configuration, XElement target, Adaptable source)
        {
            string searchValue = null;
            if (!string.IsNullOrWhiteSpace(configuration.SearchPath))
            {
                var searchAdaptablePath = AdaptablePathContainer.CreateAdaptablePath(configuration.SearchPath);

                Adaptable searchPathTarget = source.NavigateToAdaptable(searchAdaptablePath.CreatePathQueue());
                searchValue = searchPathTarget.GetValue(searchAdaptablePath.PropertyName);
            }

            string actualAdaptablePath = string.IsNullOrWhiteSpace(searchValue) ? configuration.AdaptablePath : configuration.AdaptablePath.Replace("{{searchResult}}", searchValue);
            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(actualAdaptablePath);

            Adaptable pathTarget = source.NavigateToAdaptable(adaptablePathContainer.CreatePathQueue());
            string value = pathTarget.GetValue(adaptablePathContainer.PropertyName);

            target.SetXPathValues(configuration.XPath, value);
        }
    }
}