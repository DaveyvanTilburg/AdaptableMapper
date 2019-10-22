using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace XPathSerialization.XPathConfigurations
{
    internal class XPathScope : XPathConfigurationBase, XPathConfiguration
    {
        private const string TYPE = "Scope";
        public string Type => TYPE;

        public string SearchPath => string.Empty;

        public XPathScope(string xPath, string adaptablePath) : base(xPath, adaptablePath) { }

        public override void DeSerialize(XElement source, Adaptable target)
        {
            IEnumerable<XElement> xScope = source.NavigateToPathSelectAll(XPath);

            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(AdaptablePath);

            Adaptable pathTarget = target.GetOrCreateAdaptable(adaptablePathContainer.GetPath());
            IList adaptableScope = pathTarget.GetListProperty(adaptablePathContainer.PropertyName);

            foreach (XElement xElement in xScope)
            {
                Adaptable entry = adaptableScope.GetType().CreateAdaptable();
                entry.SetParent(pathTarget);

                foreach (XPathConfigurationBase xPathConfiguration in XPathConfigurations)
                    xPathConfiguration.DeSerialize(xElement, entry);

                adaptableScope.Add(entry);
            }
        }

        public override void Serialize(XElement target, Adaptable source)
        {
            XElement xTemplate = target.NavigateToPath(XPath);
            XElement xParent = xTemplate.GetParent();
            xTemplate.Remove();

            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(AdaptablePath);

            Adaptable pathTarget = source.NavigateToAdaptable(adaptablePathContainer.GetPath());
            IList adaptableScope = pathTarget.GetListProperty(adaptablePathContainer.PropertyName);

            foreach (Adaptable sourceItem in adaptableScope)
            {
                var copy = new XElement(xTemplate);
                foreach (XPathConfigurationBase xPathConfiguration in XPathConfigurations)
                    xPathConfiguration.Serialize(copy, sourceItem);

                xParent.Add(copy);
            }
        }
    }
}