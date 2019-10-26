using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Linq;
using System.Collections;

namespace AdaptableMapper
{
    internal static class XElementExtensions
    {
        public static IReadOnlyCollection<XElement> NavigateToPathSelectAll(this XElement xElement, string xPath)
        {
            IReadOnlyCollection<XElement> allMatches = xElement.XPathSelectElements(xPath).ToList();

            if (!allMatches.Any())
            {
                Errors.ErrorObservable.GetInstance().Raise($"Path could not be traversed : {xPath}");
                return new List<XElement>();
            }

            return allMatches;
        }

        public static XElement NavigateToPath(this XElement xElement, string xPath)
        {
            IList<XElement> allMatches = xElement.XPathSelectElements(xPath).ToList();

            if(!allMatches.Any())
                Errors.ErrorObservable.GetInstance().Raise($"Path could not be traversed : {xPath}");

            if(allMatches.Count > 1)
                Errors.ErrorObservable.GetInstance().Raise($"Path has multiple of the same node, when only one is expected : {xPath}");

            return allMatches?.First() ?? new XElement(string.Empty);
        }

        public static IEnumerable<string> GetXPathValues(this XElement xElement, string xPath)
        {
            var enumerable = xElement.XPathEvaluate(xPath) as IEnumerable;
            var xObjects = enumerable.Cast<XObject>();

            if (!xObjects.Any())
                Errors.ErrorObservable.GetInstance().Raise($"Path could not be traversed : {xPath}");
            else
            {
                foreach (XObject xObject in xObjects)
                {
                    if (xObject is XElement element)
                        yield return element.Value;
                    else if (xObject is XAttribute attribute)
                        yield return attribute.Value;
                }
            }
        }

        public static string GetXPathValue(this XElement xElement, string xPath)
        {
            var enumerable = xElement.XPathEvaluate(xPath) as IEnumerable;
            var xObject = enumerable.Cast<XObject>().FirstOrDefault();

            if (xObject == null)
            {
                Errors.ErrorObservable.GetInstance().Raise($"Path could not be traversed : {xPath}");
                return string.Empty;
            }

            if (xObject is XElement element)
                return element.Value;
            else if (xObject is XAttribute attribute)
                return attribute.Value;

            Errors.ErrorObservable.GetInstance().Raise($"Path did not end in an attribute or element : {xPath}");
            return string.Empty;
        }

        public static void SetXPathValues(this XElement xElement, string xPath, string value)
        {
            var enumerable = xElement.XPathEvaluate(xPath) as IEnumerable;
            var xObjects = enumerable.Cast<XObject>();

            if (!xObjects.Any())
                Errors.ErrorObservable.GetInstance().Raise($"Path could not be traversed : {xPath}");
            else
            {
                foreach (XObject xObject in xObjects)
                {
                    if (xObject is XElement element)
                        element.Value = value;
                    else if (xObject is XAttribute attribute)
                        attribute.Value = value;
                }
            }
        }

        public static XElement GetParent(this XElement xElement)
        {
            if (xElement.Parent == null)
                Errors.ErrorObservable.GetInstance().Raise($"parent of node {xElement} is null");

            return xElement?.Parent ?? new XElement(string.Empty);
        }
    }
}