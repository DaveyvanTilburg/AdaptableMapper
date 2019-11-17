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
            IReadOnlyCollection<XElement> allMatches;
            try
            {
                allMatches = xElement.XPathSelectElements(xPath).ToList();
            }
            catch(XPathException exception)
            {
                Process.ProcessObservable.GetInstance().Raise("XML#28; Path is invalid", "error", xPath, xElement, exception.GetType().Name, exception.Message);
                return new List<XElement>();
            }

            return allMatches;
        }

        public static XElement NavigateToPath(this XElement xElement, string xPath)
        {
            IReadOnlyCollection<XElement> allMatches;

            try
            {
                allMatches = xElement.XPathSelectElements(xPath).ToList();
            }
            catch (XPathException exception)
            {
                Process.ProcessObservable.GetInstance().Raise("XML#27; Path is invalid", "error", xPath, xElement, exception.GetType().Name, exception.Message);
                return new XElement("nullObject");
            }

            if(!allMatches.Any())
            {
                Process.ProcessObservable.GetInstance().Raise("XML#2; Path could not be traversed", "warning", xPath, xElement);
                return new XElement("nullObject");
            }

            if (allMatches.Count > 1)
            {
                Process.ProcessObservable.GetInstance().Raise("XML#3; Path has multiple of the same node, when only one is expected", "warning", xPath, xElement);
                return new XElement("nullObject");
            }

            return allMatches.FirstOrDefault();
        }

        public static string GetXPathValue(this XElement xElement, string xPath)
        {
            string result = string.Empty;

            IEnumerable enumerable;
            try
            {
                enumerable = xElement.XPathEvaluate(xPath) as IEnumerable;
            }
            catch (XPathException exception)
            {
                Process.ProcessObservable.GetInstance().Raise("XML#29; Path is invalid", "error", xPath, xElement, exception.GetType().Name, exception.Message);
                return string.Empty;
            }

            var xObject = enumerable.Cast<XObject>().FirstOrDefault();

            if (xObject == null)
                return string.Empty;

            if (xObject is XElement element)
                result = element.Value;
            if (xObject is XAttribute attribute)
                result = attribute.Value;
            
            return result;
        }

        public static void SetXPathValues(this XElement xElement, string xPath, string value)
        {
            IEnumerable enumerable;

            try
            {
                enumerable = xElement.XPathEvaluate(xPath) as IEnumerable;
            }
            catch (XPathException)
            {
                enumerable = new List<XElement>();
            }

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
    }
}