using System.Xml.Linq;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Languages.Xml.Interpretation;
using MappingFramework.Traversals;

namespace MappingFramework.Languages.Xml.Traversals
{
    [ContentType(ContentType.Xml)]
    public sealed class XmlSetValueTraversal : SetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "5052f42d-894d-4215-a5f5-b86b8af89860";
        public string TypeId => _typeId;

        public XmlSetValueTraversal() { }
        public XmlSetValueTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
        public XmlInterpretation XmlInterpretation { get; set; }
        public bool SetAsCData { get; set; }

        public void SetValue(Context context, string value)
        {
            XElement xElement = (XElement)context.Target;
            xElement.SetXPathValues(Path.ConvertToInterpretation(XmlInterpretation), value, SetAsCData, context);
        }
    }
}