﻿using System.Collections.Generic;
using System.Xml.Linq;
using XPathSerialization.XPathConfigurations;

namespace XPathSerialization
{
    public abstract class XPathConfiguration
    {
        protected string XPath;
        protected string AdaptablePath;
        protected IList<XPathConfiguration> XPathConfigurations = new List<XPathConfiguration>();

        protected XPathConfiguration(string xPath, string objectPath)
        {
            XPath = xPath;
            AdaptablePath = objectPath;
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

        public abstract void DeSerialize(XElement source, Adaptable target);

        public abstract void Serialize(XElement target, Adaptable source);
    }
}