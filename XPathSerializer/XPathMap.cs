using System.Collections.Generic;
using System.Xml.Linq;

namespace XPathSerialization
{
    internal class XPathMap : XPathConfiguration
    {
        public XPathMap(string xPath, string objectPath) : base(xPath, objectPath) { }

        public override void DeSerialize(XElement source, Adaptable target)
        {
            IEnumerable<string> elementValues = source.GetXPathValues(XPath);

            foreach (string elementValue in elementValues)
                target.SetPropertyValue(ObjectPath, elementValue);
        }

        public override void Serialize(XElement target, Adaptable source)
        {
            var value = source.GetPropertyValue(ObjectPath);

            target.SetXPathValues(XPath, value);
        }
    }
}