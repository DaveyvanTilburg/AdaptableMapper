using System.Xml.Linq;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlSetValueNamespacelessTraversal : SetValueTraversal
    {
        public XmlSetValueNamespacelessTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public void SetValue(object target, string value)
        {
            if (!(target is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#36; target is not of expected type XElement", "error", Path, target?.GetType().Name);
                return;
            }

            xElement.SetXPathValues(Path.ConvertToNamespacelessPath(), value);
        }
    }
}