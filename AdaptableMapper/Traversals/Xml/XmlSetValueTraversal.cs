using System.Xml.Linq;
using AdaptableMapper.Configuration;
using AdaptableMapper.Xml;
using Newtonsoft.Json;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlSetValueTraversal : SetMutableValueTraversal
    {
        [JsonConstructor]
        public XmlSetValueTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
        public XmlInterpretation XmlInterpretation { get; set; }

        protected override void SetValueImplementation(Context context, string value)
        {
            if (!(context.Target is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#21; target is not of expected type XElement", "error", Path, context.Target?.GetType().Name);
                return;
            }

            xElement.SetXPathValues(Path.ConvertToInterpretation(XmlInterpretation), value);
        }
    }
}