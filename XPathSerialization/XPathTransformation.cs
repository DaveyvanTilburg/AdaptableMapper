using System.Xml.Linq;

namespace XPathSerialization
{
    public interface XPathTransformation
    {
        void Serialize(XPathConfiguration configuration, XElement source, Adaptable target);
        void Deserialize(XPathConfiguration configuration, XElement target, Adaptable source);
    }
}