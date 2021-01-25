using System.Xml.Linq;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Xml;

namespace MappingFramework.Traversals.Xml
{
    [ContentType(ContentType.Xml)]
    public sealed class XmlGetValueTraversal : GetSearchPathValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "b3a8e531-2a44-42e3-bac4-1f6b0c1b80b3";
        public string TypeId => _typeId;

        public XmlGetValueTraversal() { }
        public XmlGetValueTraversal(string path)
        {
            Path = path;
        }

        public XmlGetValueTraversal(string path, XmlInterpretation xmlInterpretation)
        {
            Path = path;
            XmlInterpretation = xmlInterpretation;
        }

        public string Path { get; set; }
        public XmlInterpretation XmlInterpretation { get; set; }

        public string GetValue(Context context)
        {
            XElement xElement = (XElement)context.Source;
            MethodResult<string> result = xElement.GetXPathValue(Path.ConvertToInterpretation(XmlInterpretation), context);

            if (!result.IsValid)
                return string.Empty;

            return result.Value;
        }
    }
}