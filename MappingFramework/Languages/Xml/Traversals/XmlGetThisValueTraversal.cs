using System.Xml.Linq;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;

namespace MappingFramework.Languages.Xml.Traversals
{
    [ContentType(ContentType.Xml)]
    public sealed class XmlGetThisValueTraversal : GetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "2be460d7-4f86-4b72-983b-09b323d63abf";
        public string TypeId => _typeId;

        public XmlGetThisValueTraversal() { }

        public string GetValue(Context context)
        {
            XElement xElement = (XElement)context.Source;
            return xElement.Value;
        }
    }
}