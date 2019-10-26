using AdaptableMapper.Traversals;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public class XmlScope : GetScopeTraversal
    {
        public string Path { get; set; }

        public IEnumerable<object> GetScope(object source)
        {
            if (!(source is XElement xElement))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type XElement");
                return new List<XElement>();
            }

            IEnumerable<XElement> xScope = xElement.NavigateToPathSelectAll(Path);
            return xScope;
        }
    }
}