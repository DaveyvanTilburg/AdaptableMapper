using System.Collections.Generic;
using System.Xml.Linq;

namespace XPathSerialization
{
    public abstract class XPathConfiguration
    {
        protected string XPath;
        protected string ObjectPath;
        protected IList<XPathConfiguration> XPathConfigurations = new List<XPathConfiguration>();

        protected XPathConfiguration(string xPath, string objectPath)
        {
            XPath = xPath;
            ObjectPath = objectPath;
        }

        public static XPathConfiguration CreateXPathMap(string xPath, string objectPath)
        {
            return new XPathMap(xPath, objectPath);
        }

        public static XPathConfiguration CreateXPathScope(string xPath, string objectPath)
        {
            return new XPathScope(xPath, objectPath);
        }

        public static XPathConfiguration CreateXPathSearch(string xPath, string objectPath, string searchPath)
        {
            return new XPathSearch(xPath, objectPath, searchPath);
        }

        public void SetConfigurations(IList<XPathConfiguration> xPathConfigurations)
        {
            XPathConfigurations = xPathConfigurations;
        }

        public abstract void DeSerialize(XElement root, Adaptable adaptable);
    }
}