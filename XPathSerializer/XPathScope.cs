using System.Collections;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XPathSerialization
{
    internal class XPathScope : XPathConfiguration
    {
        public override void DeSerialize(XElement root, Adaptable adaptable)
        {
            var xScope = root.XPathSelectElements(XPath);
            IList objectScope = adaptable.GetProperty(ObjectPath);

            foreach (XElement xElement in xScope)
            {
                Adaptable entry = objectScope.GetType().CreateAdaptable();

                foreach (XPathConfiguration xPathConfiguration in XPathConfigurations)
                    xPathConfiguration.DeSerialize(xElement, entry);

                objectScope.Add(entry);
            } 
        }
    }
}