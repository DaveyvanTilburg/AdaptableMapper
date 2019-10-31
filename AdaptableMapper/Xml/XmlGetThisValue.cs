using AdaptableMapper.Traversals;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlGetThisValue : GetValueTraversal
    {
        public string GetValue(object source)
        {
            if (!(source is XElement xElement))
            {
                Errors.ErrorObservable.GetInstance().Raise("XML#16; source is not of expected type XElement", source);
                return string.Empty;
            }

            return xElement.Value;
        }
    }
}