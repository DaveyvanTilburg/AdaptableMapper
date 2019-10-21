using System.Collections.Generic;

namespace XPathSerialization
{
    public interface XPathConfigurationInternalsTEMP
    {
        string Type { get; }
        string XPath { get; }
        string AdaptablePath { get; }
        string SearchPath { get; }
        IList<XPathConfiguration> XPathConfigurations { get; }
    }
}