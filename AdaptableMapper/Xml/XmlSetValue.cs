using AdaptableMapper.Traversals;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlSetValue : SetValueTraversal
    {
        public XmlSetValue(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public void SetValue(object target, string value)
        {
            if (!(target is XElement xElement))
            {
                Errors.ErrorObservable.GetInstance().Raise("XML#21; target is not of expected type XElement", Path, target?.GetType()?.Name);
                return;
            }

            xElement.SetXPathValues(Path, value);
        }
    }
}