using AdaptableMapper.Traversals;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlSetThisValueTraversal : SetValueTraversal
    {
        public void SetValue(object target, string value)
        {
            if (!(target is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#20; target is not of expected type XElement", "error", target?.GetType().Name);
                return;
            }

            xElement.Value = value;
        }
    }
}