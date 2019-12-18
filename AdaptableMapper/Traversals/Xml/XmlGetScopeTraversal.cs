using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AdaptableMapper.Xml;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlGetScopeTraversal : GetScopeTraversal
    {
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