using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XPathSerialization
{
    internal class XPathMap : XPathConfiguration
    {
        public override void DeSerialize(XElement root, Adaptable adaptable)
        {
            IEnumerable<string> elementValues = GetXPathValues(root, XPath);

            foreach (string elementValue in elementValues)
                adaptable.SetPropertyValue(ObjectPath, elementValue);
        }

        private static IEnumerable<string> GetXPathValues(XElement node, string xPath)
        {
            foreach (XObject xObject in (IEnumerable)node.XPathEvaluate(xPath))
            {
                if (xObject is XElement element)
                    yield return element.Value;
                else if (xObject is XAttribute attribute)
                    yield return attribute.Value;
            }
        }
    }
}