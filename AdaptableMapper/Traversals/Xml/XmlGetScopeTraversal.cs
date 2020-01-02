using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AdaptableMapper.Converters;
using AdaptableMapper.Xml;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlGetScopeTraversal : GetScopeTraversal, ResolvableByTypeId
    {
        public const string _typeId = "4b9876c8-8c60-40fd-9141-86688a44dbe1";
        public string TypeId => _typeId;

        public XmlGetScopeTraversal() { }
        public XmlGetScopeTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
        public XmlInterpretation XmlInterpretation { get; set; }

        public MethodResult<IEnumerable<object>> GetScope(object source)
        {
            if (!(source is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#12; source is not of expected type XElement", "error", Path, source?.GetType().Name);
                return new NullMethodResult<IEnumerable<object>>();
            }

            IEnumerable<XElement> xScope = xElement.NavigateToPathSelectAll(Path.ConvertToInterpretation(XmlInterpretation));
            if (!xScope.Any())
            {
                Process.ProcessObservable.GetInstance().Raise("XML#5; Path resulted in no items", "warning", Path, source.GetType().Name);
                return new NullMethodResult<IEnumerable<object>>();
            }

            return new MethodResult<IEnumerable<object>>(xScope);
        }
    }
}