using System.Collections.Generic;

namespace Web.Controllers.XPath
{
    public class XPathConfigurationEntry
    {
        public string Type { get; set; } = string.Empty;
        public string XPath { get; set; } = string.Empty;
        public string AdaptablePath { get; set; } = string.Empty;
        public string SearchPath { get; set; } = string.Empty;
        public List<XPathConfigurationEntry> Configurations { get; set; } = new List<XPathConfigurationEntry>();
    }
}