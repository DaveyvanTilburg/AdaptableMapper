using System.Xml.Linq;

namespace XPathSerialization.XPathConfigurations
{
    internal class XPathMap : XPathConfigurationBase, XPathConfiguration
    {
        private const string TYPE = "Map";
        public string Type => TYPE;

        public string SearchPath => string.Empty;

        public XPathMap(string xPath, string adaptablePath) : base(xPath, adaptablePath) { }

        public override void DeSerialize(XElement source, Adaptable target)
        {
            string value = source.GetXPathValue(XPath);

            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(AdaptablePath);
            Adaptable pathTarget = target.GetOrCreateAdaptable(adaptablePathContainer.GetPath());

            pathTarget.SetValue(adaptablePathContainer.PropertyName, value);
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