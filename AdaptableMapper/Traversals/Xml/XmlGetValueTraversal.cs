using System.Xml.Linq;
using AdaptableMapper.Xml;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlGetValueTraversal : GetValueTraversal
    {
        public XmlInterpretation XmlInterpretation { get; set; }

        public XmlGetValueTraversal(string path)
        {
            XmlInterpretation = XmlInterpretation.Default;
            Path = path;
        }

        public string Path { get; set; }

        public string GetValue(object source)
        {
            if (!(source is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#17; source is not of expected type XElement", "error", Path, source?.GetType().Name);
                return string.Empty;
            }

            string actualPath;
            if (XmlInterpretation == XmlInterpretation.WithoutNamespace)
                actualPath = Path.ConvertToNamespacelessPath();
            else
                actualPath = Path;

            MethodResult<string> result = xElement.GetXPathValue(actualPath);
            if (result.IsValid && string.IsNullOrWhiteSpace(result.Value))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#4; Path resulted in no items", "warning", Path, xElement);
                return string.Empty;
            }

            if (!result.IsValid)
                return string.Empty;

            return result.Value;
        }
    }
}