using System.Xml.Linq;
using AdaptableMapper.Xml;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlSetValueTraversal : SetValueTraversal
    {
        public XmlInterpretation XmlInterpretation { get; set; }

        public XmlSetValueTraversal(string path)
        {
            XmlInterpretation = XmlInterpretation.Default;
            Path = path;
        }

        public string Path { get; set; }

        public void SetValue(object target, string value)
        {
            if (!(target is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#21; target is not of expected type XElement", "error", Path, target?.GetType().Name);
                return;
            }

            string actualPath;
            if (XmlInterpretation == XmlInterpretation.WithoutNamespace)
                actualPath = Path.ConvertToNamespacelessPath();
            else
                actualPath = Path;

            xElement.SetXPathValues(actualPath, value);
        }
    }
}