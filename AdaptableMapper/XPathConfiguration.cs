using System.Collections.Generic;

namespace AdaptableMapper
{
    public class XPathConfiguration
    {
        public string Type { get; set; }
        public string XPath { get; set; }
        public string AdaptablePath { get; set; }
        public string SearchPath { get; set; }
        public List<XPathConfiguration> Configurations { get; set; } = new List<XPathConfiguration>();

        public static XPathConfiguration CreateMapConfiguration(string xPath, string adaptablePath)
        {
            var result = new XPathConfiguration()
            {
                Type = "Map",
                XPath = xPath,
                AdaptablePath = adaptablePath
            };
            return result;
        }

        public static XPathConfiguration CreateSearchConfiguration(string xPath, string adaptablePath, string searchPath)
        {
            var result = new XPathConfiguration()
            {
                Type = "Search",
                XPath = xPath,
                AdaptablePath = adaptablePath,
                SearchPath = searchPath
            };
            return result;
        }

        public static XPathConfiguration CreateScopeConfiguration(string xPath, string adaptablePath, List<XPathConfiguration> configurations)
        {
            var result = new XPathConfiguration()
            {
                Type = "Scope",
                XPath = xPath,
                AdaptablePath = adaptablePath,
                Configurations = configurations
            };
            return result;
        }
    }
}