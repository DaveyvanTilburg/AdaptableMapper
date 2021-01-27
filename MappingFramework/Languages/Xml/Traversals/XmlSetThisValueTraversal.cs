using System.Xml.Linq;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;

namespace MappingFramework.Languages.Xml.Traversals
{
    [ContentType(ContentType.Xml)]
    public sealed class XmlSetThisValueTraversal : SetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "0e33e050-8da6-4d87-ad49-ff9bde9bf953";
        public string TypeId => _typeId;

        public XmlSetThisValueTraversal() { }

        public bool SetAsCData { get; set; }

        public void SetValue(Context context, string value)
        {
            XElement xElement = (XElement)context.Target;

            if (SetAsCData)
                xElement.Add(new XCData(value));
            else
                xElement.Value = value;
        }
    }
}