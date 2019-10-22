using System.Collections.Generic;
using System.Xml.Linq;

namespace XPathSerialization
{
    public interface XPathConfiguration
    {
        string Type { get; }
        string XPath { get; }
        string AdaptablePath { get; }
        string SearchPath { get; }
        IList<XPathConfiguration> XPathConfigurations { get; }

        void SetConfigurations(IList<XPathConfiguration> xPathConfigurations);
        void DeSerialize(XElement source, Adaptable target);
        void Serialize(XElement target, Adaptable source);
    }
}