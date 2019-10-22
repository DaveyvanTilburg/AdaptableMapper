using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
                current = XPathConfigurationBase.CreateXPathScope(step.XPath, step.AdaptablePath);
            else if (step.Type.Equals("Map"))
                current = XPathConfigurationBase.CreateXPathMap(step.XPath, step.AdaptablePath);
            else if (step.Type.Equals("Search"))
                current = XPathConfigurationBase.CreateXPathSearch(step.XPath, step.AdaptablePath, step.SearchPath);

            if (current == null)
                throw new Exception($"Type is empty of entry {JsonConvert.SerializeObject(step)}");

            var children = step.Configurations?
                .Select(Convert)
                .ToList();

            current.SetConfigurations(children);

            return current;
        }

        public static XPathConfigurationEntry Convert(XPathConfiguration step)
        {
            var exposed = step as XPathConfiguration;

            var current = new XPathConfigurationEntry()
            {
                Type = exposed.Type,
                XPath = exposed.XPath,
                AdaptablePath = exposed.AdaptablePath,
                SearchPath = exposed.SearchPath
            };

            var children = exposed.XPathConfigurations?
                .Select(Convert)
                .ToList();

            current.Configurations = children;

            return current;
        }
    }
}