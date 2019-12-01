using System.Xml.Linq;
using AdaptableMapper.Formats;
using AdaptableMapper.Xml;
using Newtonsoft.Json;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlSetValueTraversal : SetFormattedValueTraversal
    {
        public XmlInterpretation XmlInterpretation { get; set; }

        [JsonConstructor]
        public XmlSetValueTraversal(string path) : base(new NullFormatter())
        {
            Path = path;
        }

        public XmlSetValueTraversal(string path, Formatter formatter) : base(formatter)
        {
            XmlInterpretation = XmlInterpretation.Default;
            Path = path;
        }

        public string Path { get; set; }

        protected override void SetValueImplementation(object target, string value)
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