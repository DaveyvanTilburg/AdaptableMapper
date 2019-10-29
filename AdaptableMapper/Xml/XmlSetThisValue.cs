using AdaptableMapper.Traversals;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlSetThisValue : SetValueTraversal
    {
        public void SetValue(object target, string value)
        {
            if (!(target is XElement xElement))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type XElement");
                return;
            }

            xElement.Value = value;
        }
    }
}