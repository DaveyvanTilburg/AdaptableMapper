using System.Xml.Linq;
using AdaptableMapper.Configuration;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlSetThisValueTraversal : SetValueTraversal
    {
        public void SetValue(Context context, string value)
        {
            if (!(context.Target is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#20; target is not of expected type XElement", "error", context.Target?.GetType().Name);
                return;
            }

            xElement.Value = value;
        }
    }
}