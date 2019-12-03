using System.Xml;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    internal class NullElement : XElement
    {
        private NullElement() : base("nullElement") { }

        public override XmlNodeType NodeType => XmlNodeType.None;

        public static XElement Create()
        {
            return new XDocument(new NullElement()).Root;
        }
    }
}