using System.Linq;
using System.Xml.Linq;

namespace XPathSerialization.XPathConfigurations
{
    internal partial class XPathMap : XPathConfiguration
    {
        public XPathMap(string xPath, string adaptablePath) : base(xPath, adaptablePath) { }

        public override void DeSerialize(XElement source, Adaptable target)
        {
            string elementValue = source.GetXPathValues(XPath).First();

            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(AdaptablePath);

            Adaptable pathTarget = target.GetOrCreateAdaptable(adaptablePathContainer.GetPath());
            pathTarget.SetValue(adaptablePathContainer.PropertyName, elementValue);
        }

        public override void Serialize(XElement target, Adaptable source)
        {
            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(AdaptablePath);

            Adaptable pathTarget = source.NavigateToAdaptable(adaptablePathContainer.GetPath());
            string value = pathTarget.GetValue(adaptablePathContainer.PropertyName);

            target.SetXPathValues(XPath, value);
        }
    }
}