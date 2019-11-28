using AdaptableMapper.Traversals;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlGetThisValueTraversal : GetValueTraversal
    {
        public string GetValue(object source)
        {
            if (!(source is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#16; source is not of expected type XElement", "error", source?.GetType().Name);
                return string.Empty;
            }

            return xElement.Value;
        }
    }
}