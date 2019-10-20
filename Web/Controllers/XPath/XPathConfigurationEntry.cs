using System.Collections.Generic;
using System.Linq;
using XPathSerialization;

namespace Web.Controllers.XPath
{
    public class XPathConfigurationEntry
    {
        public string Type { get; set; } = string.Empty;
        public string XPath { get; set; } = string.Empty;
        public string AdaptablePath { get; set; } = string.Empty;
        public string SearchPath { get; set; } = string.Empty;
        public List<XPathConfigurationEntry> Configurations { get; set; } = new List<XPathConfigurationEntry>();

        public static XPathConfiguration Convert(XPathConfigurationEntry step)
        {
            XPathConfiguration current = null;
            if (step.Type.Equals("Scope"))
                current = XPathConfiguration.CreateXPathScope(step.XPath, step.AdaptablePath);
            else if (step.Type.Equals("Map"))
                current = XPathConfiguration.CreateXPathMap(step.XPath, step.AdaptablePath);
            else if (step.Type.Equals("Search"))
                current = XPathConfiguration.CreateXPathSearch(step.XPath, step.AdaptablePath, step.SearchPath);

            var children = step.Configurations?
                .Select(c => Convert(c))
                .ToList();

            current.SetConfigurations(children);

            return current;
        }

        public static XPathConfigurationEntry Convert(XPathConfiguration step)
        {
            XPathConfigurationInternalsTEMP exposed = step as XPathConfigurationInternalsTEMP;

            var current = new XPathConfigurationEntry()
            {
                Type = exposed.Type,
                XPath = exposed.XPath,
                AdaptablePath = exposed.AdaptablePath,
                SearchPath = exposed.SearchPath
            };

            var children = exposed.XPathConfigurations?
                .Select(c => Convert(c))
                .ToList();

            current.Configurations = children;

            return current;
        }
    }
}