using System.Xml.Linq;
using XPathSerialization.Navigations;

namespace XPathSerialization.XPathConfigurations
{
    internal class XPathMap
    {
        public void Serialize(Navigation source, Navigation target, Context context)
        {
            string value = source.GetValue(context.Source);

            target.SetValue(context.Target, value);
        }
    }

    internal class XPathMap : XPathTransformation
    {
        public void Serialize(XPathConfiguration configuration, XElement source, Adaptable target)
        {
            string value = source.GetXPathValue(configuration.XPath);

            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(configuration.AdaptablePath);
            Adaptable pathTarget = target.GetOrCreateAdaptable(adaptablePathContainer.GetPath());

            pathTarget.SetValue(adaptablePathContainer.PropertyName, value);
        }

        public void Deserialize(XPathConfiguration configuration, XElement target, Adaptable source)
        {
            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(configuration.AdaptablePath);

            Adaptable pathTarget = source.NavigateToAdaptable(adaptablePathContainer.GetPath());
            string value = pathTarget.GetValue(adaptablePathContainer.PropertyName);

            target.SetXPathValues(configuration.XPath, value);
        }
    }
}