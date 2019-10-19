using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XPathSerialization
{
    internal static class XElementExtensions
    {
        public static IEnumerable<string> GetXPathValues(this XElement node, string xPath)
        {
            foreach (XObject xObject in (IEnumerable)node.XPathEvaluate(xPath))
            {
                if (xObject is XElement element)
                    yield return element.Value;
                else if (xObject is XAttribute attribute)
                    yield return attribute.Value;
            }
        }

        public static void SetXPathValues(this XElement node, string xPath, string value)
        {
            foreach (XObject xObject in (IEnumerable)node.XPathEvaluate(xPath))
            {
                if (xObject is XElement element)
                    element.Value = value;
                else if (xObject is XAttribute attribute)
                    attribute.Value = value;
            }
        }
    }
}