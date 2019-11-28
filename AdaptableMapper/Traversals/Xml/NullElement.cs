using System.Xml;
using System.Xml.Linq;

namespace AdaptableMapper.Traversals.Xml
{
    internal class NullElement : XElement
    {
        public NullElement() : base("nullElement") { }

        public override XmlNodeType NodeType => XmlNodeType.None;
    }
}