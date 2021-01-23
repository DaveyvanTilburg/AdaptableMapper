using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Linq;
using System.Collections;
using MappingFramework.Configuration;
using MappingFramework.Xml;

namespace MappingFramework.Traversals.Xml
{
    public static class XElementExtensions
    {
        public static IReadOnlyCollection<XElement> NavigateToPathSelectAll(this XElement xElement, string xPath, Context context)
        {
            IReadOnlyCollection<XElement> allMatches;
            try
            {
                allMatches = xElement.XPathSelectElements(xPath).ToList();
            }
            catch (XPathException exception)
            {
                context.NavigationException(xPath, exception);
                return new List<XElement>();
            }

            return allMatches;
        }

        public static XElement NavigateToPath(this XElement xElement, string xPath, Context context)
        {
            IReadOnlyCollection<XObject> allMatches;

            try
            {
                IEnumerable enumerable = xElement.XPathEvaluate(xPath) as IEnumerable;
                allMatches = enumerable?.Cast<XObject>().ToList();
            }
            catch (XPathException exception)
            {
                context.NavigationException(xPath, exception);
                return NullElement.Create();
            }

            if (!allMatches.Any())
            {
                context.NavigationResultIsEmpty(xPath);
                return NullElement.Create();
            }

            if (!(allMatches.First() is XElement result))
            {
                context.NavigationInvalid(xPath, "Path did not end in an element");
                return NullElement.Create();
            }

            return result;
        }

        public static MethodResult<string> GetXPathValue(this XElement xElement, string xPath, Context context)
        {
            string result = string.Empty;

            object pathResult;
            try
            {
                pathResult = xElement.XPathEvaluate(xPath);
            }
            catch (XPathException exception)
            {
                context.NavigationException(xPath, exception);
                return new NullMethodResult<string>();
            }

            if (pathResult is string stringValue)
                return new MethodResult<string>(stringValue);

            if (pathResult is IEnumerable enumerable)
            {
                object value = enumerable.Cast<object>().FirstOrDefault();

                if (value == null)
                {
                    context.NavigationResultIsEmpty(xPath);
                    return new NullMethodResult<string>();
                }

                if (value is XElement element)
                    result = element.Value;
                if (value is XAttribute attribute)
                    result = attribute.Value;
                if (value is XProcessingInstruction processingInstruction)
                    result = processingInstruction.Data;

                return new MethodResult<string>(result);
            }

            return new MethodResult<string>(pathResult.ToString());
        }

        public static void SetXPathValues(this XElement xElement, string xPath, string value, bool setAsCData, Context context)
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

            var xObjects = enumerable?.Cast<XObject>();

            if (!xObjects.Any())
                context.NavigationResultIsEmpty(xPath);
            else
            {
                foreach (XObject xObject in xObjects)
                {
                    if (xObject is XElement element)
                    {
                        if (setAsCData)
                        {
                            element.Add(new XCData(value));
                        }
                        else
                        {
                            element.Value = value;
                        }
                    }
                    else if (xObject is XAttribute attribute)
                        attribute.Value = value;
                    else if (xObject is XProcessingInstruction processingInformation)
                        processingInformation.Data = value;
                }
            }
        }
    }
}