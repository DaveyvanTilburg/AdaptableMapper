using System.Collections.Generic;

namespace Web
{
    public class XPathConfigurationEntry
    {
        public string Type { get; set; }
        public string XPath { get; set; }
        public string ObjectPath { get; set; }
        public List<XPathConfigurationEntry> Configurations { get; set; } = new List<XPathConfigurationEntry>();
    }
}