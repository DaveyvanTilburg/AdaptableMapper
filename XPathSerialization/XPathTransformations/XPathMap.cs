using System.Xml.Linq;

namespace XPathSerialization.XPathConfigurations
{
    internal class XPathMap : XPathTransformation
    {
        public void Serialize(XPathConfiguration configuration, XElement source, Adaptable target)
        {
            string value = source.GetXPathValue(configuration.XPath);

            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(configuration.AdaptablePath);
            Adaptable pathTarget = target.GetOrCreateAdaptable(adaptablePathContainer.CreatePathQueue());

            pathTarget.SetValue(adaptablePathContainer.PropertyName, value);
        }

        public void Deserialize(XPathConfiguration configuration, XElement target, Adaptable source)
        {
            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(configuration.AdaptablePath);

            Adaptable pathTarget = source.NavigateToAdaptable(adaptablePathContainer.CreatePathQueue());
            string value = pathTarget.GetValue(adaptablePathContainer.PropertyName);

            target.SetXPathValues(configuration.XPath, value);
        }
    }
}