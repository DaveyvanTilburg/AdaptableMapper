using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Linq;
using System.Collections;

namespace AdaptableMapper.Xml
{
    internal static class XElementExtensions
    {
        public static IReadOnlyCollection<XElement> NavigateToPathSelectAll(this XElement xElement, string xPath)
        {
            IReadOnlyCollection<XElement> allMatches = xElement.XPathSelectElements(xPath).ToList();

            if (!allMatches.Any())
            {
                Process.ProcessObservable.GetInstance().Raise("XML#1; Path could not be traversed", "warning", xPath, xElement);
                return new List<XElement>();
            }

            return allMatches;
        }

        public static XElement NavigateToPath(this XElement xElement, string xPath)
        {
            IList<XElement> allMatches = xElement.XPathSelectElements(xPath).ToList();

            if(!allMatches.Any())
                Process.ProcessObservable.GetInstance().Raise("XML#2; Path could not be traversed", "warning", xPath, xElement);

            if(allMatches.Count > 1)
                Process.ProcessObservable.GetInstance().Raise("XML#3; Path has multiple of the same node, when only one is expected", "warning", xPath, xElement);

            return allMatches.FirstOrDefault() ?? new XElement("nullObject");
        }

        public static IEnumerable<string> GetXPathValues(this XElement xElement, string xPath)
        {
            var enumerable = xElement.XPathEvaluate(xPath) as IEnumerable;
            var xObjects = enumerable.Cast<XObject>();

            if (!xObjects.Any())
            {
                Process.ProcessObservable.GetInstance().Raise("XML#4; Path resulted in no items", "warning", xPath, xElement);
                yield break;
            }

            foreach (XObject xObject in xObjects)
            {
                if (xObject is XElement element)
                    yield return element.Value;
                else if (xObject is XAttribute attribute)
                    yield return attribute.Value;
                else
                    Process.ProcessObservable.GetInstance().Raise("XML#5; Path yielded in an item that is not an xElement or xAttribute", "warning", xPath, xElement);
            }
        }

        public static string GetXPathValue(this XElement xElement, string xPath)
        {
            IEnumerable<string> values = GetXPathValues(xElement, xPath);
            string result = values.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(result))
                return string.Empty;
            
            return result;
        }

        public static void SetXPathValues(this XElement xElement, string xPath, string value)
        {
            var enumerable = xElement.XPathEvaluate(xPath) as IEnumerable;
            var xObjects = enumerable.Cast<XObject>();

            if (!xObjects.Any())
                Process.ProcessObservable.GetInstance().Raise("XML#7; Path could not be traversed", "warning", xPath, xElement);
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
                Process.ProcessObservable.GetInstance().Raise("XML#8; attempting to access parent of node that is null", "warning", xElement);

            return xElement.Parent ?? new XElement("nullObject");
        }
    }
}