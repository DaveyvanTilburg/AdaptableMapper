using System.Xml.Linq;
using AdaptableMapper.Xml;
using Newtonsoft.Json;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlSetValueTraversal : SetMutableValueTraversal
    {
        public XmlInterpretation XmlInterpretation { get; set; }
        public string Path { get; set; }

        [JsonConstructor]
        public XmlSetValueTraversal(string path)
        {
            Path = path;
        }

        protected override void SetValueImplementation(object target, string value)
        {
            if (!(target is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#21; target is not of expected type XElement", "error", Path, target?.GetType().Name);
                return;
            }

            xElement.SetXPathValues(Path.ConvertToInterpretation(XmlInterpretation), value);
        }
    }
}