using System.Xml.Linq;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Xml;

namespace MappingFramework.Traversals.Xml
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

        public void SetValue(Context context, MappingCaches mappingCaches, string value)
        {
            if (!(context.Target is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#21; target is not of expected type XElement", "error", Path, context.Target?.GetType().Name);
                return;
            }

            xElement.SetXPathValues(Path.ConvertToInterpretation(XmlInterpretation), value, SetAsCData);
        }
    }
}