using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XPathSerialization.XPathConfigurations
{
    internal partial class XPathScope : XPathConfiguration
    {
        public XPathScope(string xPath, string objectPath) : base(xPath, objectPath) { }

        public override void DeSerialize(XElement source, Adaptable target)
        {
            IEnumerable<XElement> xScope = source.XPathSelectElements(XPath);

            ObjectPath path = ObjectPath.CreateObjectPath(AdaptablePath);

            Adaptable pathTarget = target.GetOrCreateAdaptable(new Queue<string>(path.Path));
            IList objectScope = pathTarget.GetListProperty(path.PropertyName);

            foreach (XElement xElement in xScope)
            {
                Adaptable entry = objectScope.GetType().CreateAdaptable();
                entry.SetParent(pathTarget);

                foreach (XPathConfiguration xPathConfiguration in XPathConfigurations)
                    xPathConfiguration.DeSerialize(xElement, entry);

                objectScope.Add(entry);
            } 
        }

        public override void Serialize(XElement target, Adaptable source)
        {
            XElement xScope = target.XPathSelectElements(XPath).First();

            ObjectPath path = ObjectPath.CreateObjectPath(AdaptablePath);

            Adaptable pathTarget = source.NavigateToAdaptable(new Queue<string>(path.Path));
            IList objectScope = pathTarget.GetListProperty(path.PropertyName);

            XElement xParent = xScope.Parent;
            xScope.Remove();

            foreach (Adaptable sourceItem in objectScope)
            {
                var copy = new XElement(xScope);
                foreach (XPathConfiguration xPathConfiguration in XPathConfigurations)
                    xPathConfiguration.Serialize(copy, sourceItem);

                xParent.Add(copy);
            }
        }
    }
}