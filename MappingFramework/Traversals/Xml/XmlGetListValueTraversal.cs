using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Xml;

namespace MappingFramework.Traversals.Xml
{
    [ContentType(ContentType.Xml)]
    public sealed class XmlGetListValueTraversal : GetListSearchPathValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "4b9876c8-8c60-40fd-9141-86688a44dbe1";
        public string TypeId => _typeId;

        public XmlGetListValueTraversal() { }
        public XmlGetListValueTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
        public XmlInterpretation XmlInterpretation { get; set; }

        public MethodResult<IEnumerable<object>> GetValues(Context context)
        {
            XElement xElement = (XElement)context.Source;

            IEnumerable<XElement> xScope = xElement.NavigateToPathSelectAll(Path.ConvertToInterpretation(XmlInterpretation), context);
            if (!xScope.Any())
            {
                context.NavigationResultIsEmpty(Path);
                return new NullMethodResult<IEnumerable<object>>();
            }

            return new MethodResult<IEnumerable<object>>(xScope);
        }
    }
}