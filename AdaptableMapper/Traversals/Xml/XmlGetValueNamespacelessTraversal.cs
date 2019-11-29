using System.Linq;
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

            string trimmedPath = Path.TrimStart('/', '.');
            string namespaceLessPath;
            if (Path.Contains('/'))
            {
                string[] pathParts = trimmedPath.Split('/');
                namespaceLessPath = "./*" + string.Join("/*", pathParts.Select(p => $"[local-name()='{p}']"));
            }
            else
                namespaceLessPath = $"/*[local-name()='{trimmedPath}']";
            

            MethodResult<string> result = xElement.GetXPathValue(namespaceLessPath);
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