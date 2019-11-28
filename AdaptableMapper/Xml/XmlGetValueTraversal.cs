using AdaptableMapper.Traversals;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlGetValueTraversal : GetValueTraversal
    {
        public XmlGetValueTraversal(string path)
        {
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

            MethodResult<string> result = xElement.GetXPathValue(Path);
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