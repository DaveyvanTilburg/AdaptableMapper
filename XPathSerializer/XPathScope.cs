using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XPathSerialization
{
    internal class XPathScope : XPathConfiguration
    {
        public XPathScope(string xPath, string objectPath) : base(xPath, objectPath) { }

        public override void DeSerialize(XElement source, Adaptable target)
        {
            IEnumerable<XElement> xScope = source.XPathSelectElements(XPath);
            (IList objectScope, Adaptable parent) = target.GetProperty(ObjectPath);

            foreach (XElement xElement in xScope)
            {
                Adaptable entry = objectScope.GetType().CreateAdaptable();
                entry.SetParent(parent);

                foreach (XPathConfiguration xPathConfiguration in XPathConfigurations)
                    xPathConfiguration.DeSerialize(xElement, entry);

                objectScope.Add(entry);
            } 
        }

        public override void Serialize(XElement target, Adaptable source)
        {
            XElement xScope = target.XPathSelectElements(XPath).First();
            (IList objectScope, Adaptable parent) = source.GetProperty(ObjectPath);

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