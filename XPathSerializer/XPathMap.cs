using System.Collections.Generic;
using System.Xml.Linq;

namespace XPathSerialization
{
    internal class XPathMap : XPathConfiguration
    {
        public XPathMap(string xPath, string objectPath) : base(xPath, objectPath) { }

        public override void DeSerialize(XElement root, Adaptable adaptable)
        {
            IEnumerable<string> elementValues = root.GetXPathValues(XPath);

            foreach (string elementValue in elementValues)
                adaptable.SetPropertyValue(ObjectPath, elementValue);
        }
    }
}