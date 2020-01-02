using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Linq;
using System.Collections;
using AdaptableMapper.Xml;

namespace AdaptableMapper.Traversals.Xml
{
    public static class XElementExtensions
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
                Process.ProcessObservable.GetInstance().Raise("XML#28; Path is invalid", "error", xPath, exception.GetType().Name, exception.Message);
                return new List<XElement>();
            }

            return allMatches;
        }

        public static XElement NavigateToPath(this XElement xElement, string xPath)
        {
            IReadOnlyCollection<XObject> allMatches;

            try
            {
                IEnumerable enumerable = xElement.XPathEvaluate(xPath) as IEnumerable;
                allMatches = enumerable?.Cast<XObject>().ToList();
            }
            catch (XPathException exception)
            {
                Process.ProcessObservable.GetInstance().Raise("XML#27; Path is invalid", "error", xPath, exception.GetType().Name, exception.Message);
                return NullElement.Create();
            }

            if(!allMatches.Any())
            {
                Process.ProcessObservable.GetInstance().Raise("XML#2; Path could not be traversed", "warning", xPath);
                return NullElement.Create();
            }

            if(!(allMatches.First() is XElement result))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#8; Path did not end in an element", "error", xPath);
                return NullElement.Create();
            }

            return result;
        }

        public static MethodResult<string> GetXPathValue(this XElement xElement, string xPath)
        {
            string result = string.Empty;

            object pathResult;
            try
            {
                pathResult = xElement.XPathEvaluate(xPath);
            }
            catch (XPathException exception)
            {
                Process.ProcessObservable.GetInstance().Raise("XML#29; Path is invalid", "error", xPath, exception.GetType().Name, exception.Message);
                return new NullMethodResult<string>();
            }

            if(pathResult is IEnumerable enumerable)
            {
                var xObject = enumerable.Cast<XObject>().FirstOrDefault();

                if (xObject == null)
                {
                    Process.ProcessObservable.GetInstance().Raise("XML#30; Path resulted in no items", "warning", xPath);
                    return new NullMethodResult<string>();
                }

                if (xObject is XElement element)
                    result = element.Value;
                if (xObject is XAttribute attribute)
                    result = attribute.Value;
                if (xObject is XProcessingInstruction processingInstruction)
                    result = processingInstruction.Data;

                return new MethodResult<string>(result);
            }

            return new MethodResult<string>(pathResult.ToString());
        }

        public static void SetXPathValues(this XElement xElement, string xPath, string value, bool setAsCData)
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
                Process.ProcessObservable.GetInstance().Raise("XML#7; Path could not be traversed", "warning", xPath);
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