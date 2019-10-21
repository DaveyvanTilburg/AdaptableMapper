using System.Collections.Generic;

namespace XPathSerialization.XPathConfigurations
{
    internal partial class XPathSearch : XPathConfigurationInternalsTEMP
    {
        string XPathConfigurationInternalsTEMP.Type => "Search";

        string XPathConfigurationInternalsTEMP.XPath => XPath;

        string XPathConfigurationInternalsTEMP.AdaptablePath => AdaptablePath;

        IList<XPathConfiguration> XPathConfigurationInternalsTEMP.XPathConfigurations => XPathConfigurations;

        string XPathConfigurationInternalsTEMP.SearchPath => _searchPath;
    }
}