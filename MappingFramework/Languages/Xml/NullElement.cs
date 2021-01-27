using System.Xml;
using System.Xml.Linq;

namespace MappingFramework.Languages.Xml
{
    public sealed class NullElement : XElement
    {
        private NullElement() : base("nullElement") { }

        public override XmlNodeType NodeType => XmlNodeType.None;

        public static XElement Create()
        {
            return new XDocument(new NullElement()).Root;
        }
    }
}