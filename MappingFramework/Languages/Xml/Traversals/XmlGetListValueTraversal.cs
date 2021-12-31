using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Languages.Xml.Interpretation;
using MappingFramework.Traversals;

namespace MappingFramework.Languages.Xml.Traversals
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

        string GetValueTraversalPath.Path() => Path;
        void GetValueTraversalPath.Path(string path) => Path = path;
    }
}