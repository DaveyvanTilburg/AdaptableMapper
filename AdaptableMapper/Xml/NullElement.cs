using System.Xml;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    internal class NullElement : XElement
    {
        public NullElement() : base("nullElement") { }

        public override XmlNodeType NodeType => XmlNodeType.None;
    }
}