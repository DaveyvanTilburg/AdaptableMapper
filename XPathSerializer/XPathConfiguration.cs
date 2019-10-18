using System.Collections.Generic;
using System.Xml.Linq;

namespace XPathSerialization
{
    public abstract class XPathConfiguration
    {
        protected string XPath;
        protected string ObjectPath;
        protected IList<XPathConfiguration> XPathConfigurations = new List<XPathConfiguration>();

        public static XPathConfiguration CreateXPathMap(string xPath, string objectPath)
        {
            return new XPathMap()
            {
                XPath = xPath,
                ObjectPath = objectPath
            };
        }

        public static XPathConfiguration CreateXPathScope(string xPath, string objectPath)
        {
            return new XPathScope()
            {
                XPath = xPath,
                ObjectPath = objectPath
            };
        }

        public void SetConfigurations(IList<XPathConfiguration> xPathConfigurations)
        {
            XPathConfigurations = xPathConfigurations;
        }

        public abstract void DeSerialize(XElement root, Adaptable adaptable);
    }
}