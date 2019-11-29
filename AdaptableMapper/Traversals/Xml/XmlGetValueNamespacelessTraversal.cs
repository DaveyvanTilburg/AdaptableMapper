using System.Xml.Linq;

namespace AdaptableMapper.Traversals.Xml
{
    public sealed class XmlGetValueNamespacelessTraversal : GetValueTraversal
    {
        public XmlGetValueNamespacelessTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public string GetValue(object source)
        {
            if (!(source is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#34; source is not of expected type XElement", "error", Path, source?.GetType().Name);
                return string.Empty;
            }
            

            MethodResult<string> result = xElement.GetXPathValue(Path.ConvertToNamespacelessPath());
            if (result.IsValid && string.IsNullOrWhiteSpace(result.Value))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#35; Path resulted in no items", "warning", Path, xElement);
                return string.Empty;
            }

            if (!result.IsValid)
                return string.Empty;

            return result.Value;
        }
    }
}