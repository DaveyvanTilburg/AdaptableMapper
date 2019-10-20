using System.Collections.Generic;

namespace XPathSerialization.XPathConfigurations
{
    internal partial class XPathMap : XPathConfigurationInternalsTEMP
    {
        string XPathConfigurationInternalsTEMP.Type => "Map";

        string XPathConfigurationInternalsTEMP.XPath => XPath;

        string XPathConfigurationInternalsTEMP.AdaptablePath => AdaptablePath;

        IList<XPathConfiguration> XPathConfigurationInternalsTEMP.XPathConfigurations => XPathConfigurations;

        string XPathConfigurationInternalsTEMP.SearchPath => "";
    }
}