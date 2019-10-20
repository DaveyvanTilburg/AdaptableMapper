using System.Collections.Generic;

namespace XPathSerialization.XPathConfigurations
{
    internal partial class XPathScope : XPathConfigurationInternalsTEMP
    {
        string XPathConfigurationInternalsTEMP.Type => "Scope";

        string XPathConfigurationInternalsTEMP.XPath => XPath;

        string XPathConfigurationInternalsTEMP.AdaptablePath => AdaptablePath;

        IList<XPathConfiguration> XPathConfigurationInternalsTEMP.XPathConfigurations => XPathConfigurations;

        string XPathConfigurationInternalsTEMP.SearchPath => "";
    }
}