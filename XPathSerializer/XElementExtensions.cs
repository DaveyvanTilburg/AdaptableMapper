using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Linq;

namespace XPathSerialization
{
    internal static class XElementExtensions
    {
        public static IReadOnlyCollection<XElement> NavigateToPathSelectAll(this XElement xElement, string xPath)
        {
            IReadOnlyCollection<XElement> allMatches = xElement.XPathSelectElements(xPath).ToList();

            if (!allMatches.Any())
                throw new InvalidXPathException($"Path could not be traversed : {xPath}");

            return allMatches;
        }

        public static XElement NavigateToPath(this XElement xElement, string xPath)
        {
            IList<XElement> allMatches = xElement.XPathSelectElements(xPath).ToList();

            if(!allMatches.Any())
                throw new InvalidXPathException($"Path could not be traversed : {xPath}");

            if(allMatches.Count > 0)
                throw new InvalidXPathException($"Path has multiple of the same node, when only one is expected : {xPath}");

            return allMatches.First();
        }

        public static IEnumerable<string> GetXPathValues(this XElement xElement, string xPath)
        {
            var xObjects = xElement.XPathEvaluate(xPath) as IEnumerable<XObject>;

            if (!xObjects.Any())
                throw new InvalidXPathException($"Path could not be traversed : {xPath}");

            foreach (XObject xObject in xObjects)
            {
                if (xObject is XElement element)
                    yield return element.Value;
                else if (xObject is XAttribute attribute)
                    yield return attribute.Value;
            }
        }

        public static string GetXPathValue(this XElement xElement, string xPath)
        {
            var xObjects = xElement.XPathEvaluate(xPath) as IEnumerable<XObject>;

            if (!xObjects.Any())
                throw new InvalidXPathException($"Path could not be traversed : {xPath}");

            XObject xObject = xObjects.First();

            if (xObject is XElement element)
                return element.Value;
            else if (xObject is XAttribute attribute)
                return attribute.Value;

            throw new InvalidXPathException($"Path did not end in an attribute or element : {xPath}");
        }

        public static void SetXPathValues(this XElement xElement, string xPath, string value)
        {
            var xObjects = xElement.XPathEvaluate(xPath) as IEnumerable<XObject>;

            if (!xObjects.Any())
                throw new InvalidXPathException($"Path could not be traversed : {xPath}");

            foreach (XObject xObject in xObjects)
            {
                if (xObject is XElement element)
                    element.Value = value;
                else if (xObject is XAttribute attribute)
                    attribute.Value = value;
            }
        }

        public static XElement GetParent(this XElement xElement)
        {
            if (xElement.Parent == null)
                throw new InvalidXPathException($"parent of node {xElement} is null");

            return xElement.Parent;
        }
    }
}