using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XPathSerialization.XPathConfigurations
{
    internal partial class XPathScope : XPathConfiguration
    {
        public XPathScope(string xPath, string adaptablePath) : base(xPath, adaptablePath) { }

        public override void DeSerialize(XElement source, Adaptable target)
        {
            IEnumerable<XElement> xScope = source.XPathSelectElements(XPath);

            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(AdaptablePath);

            Adaptable pathTarget = target.GetOrCreateAdaptable(adaptablePathContainer.GetPath());
            IList adaptableScope = pathTarget.GetListProperty(adaptablePathContainer.PropertyName);

            foreach (XElement xElement in xScope)
            {
                Adaptable entry = adaptableScope.GetType().CreateAdaptable();
                entry.SetParent(pathTarget);

                foreach (XPathConfiguration xPathConfiguration in XPathConfigurations)
                    xPathConfiguration.DeSerialize(xElement, entry);

                adaptableScope.Add(entry);
            } 
        }

        public override void Serialize(XElement target, Adaptable source)
        {
            XElement xScope = target.XPathSelectElements(XPath).First();

            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(AdaptablePath);

            Adaptable pathTarget = source.NavigateToAdaptable(adaptablePathContainer.GetPath());
            IList adaptableScope = pathTarget.GetListProperty(adaptablePathContainer.PropertyName);

            if (xScope.Parent == null)
                throw new InvalidXPathException($"parent of node {xScope} is null");

            XElement xParent = xScope.Parent;
            xScope.Remove();

            foreach (Adaptable sourceItem in adaptableScope)
            {
                var copy = new XElement(xScope);
                foreach (XPathConfiguration xPathConfiguration in XPathConfigurations)
                    xPathConfiguration.Serialize(copy, sourceItem);

                xParent.Add(copy);
            }
        }
    }
}