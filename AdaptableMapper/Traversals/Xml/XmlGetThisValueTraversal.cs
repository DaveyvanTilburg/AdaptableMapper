using System.Xml.Linq;
using AdaptableMapper.Configuration;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlGetThisValueTraversal : GetValueTraversal
    {
        public string GetValue(Context context)
        {
            if (!(context.Source is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#16; source is not of expected type XElement", "error", context.Source?.GetType().Name);
                return string.Empty;
            }

            return xElement.Value;
        }
    }
}