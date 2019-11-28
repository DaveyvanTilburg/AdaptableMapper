using AdaptableMapper.Traversals;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlGetSearchValueTraversal : GetValueTraversal
    {
        public XmlGetSearchValueTraversal(string searchPath, string searchValuePath)
        {
            SearchPath = searchPath;
            SearchValuePath = searchValuePath;
        }

        public string SearchPath { get; set; }
        public string SearchValuePath { get; set; }

        public string GetValue(object source)
        {
            if (!(source is XElement xElement))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#13; source is not of expected type XElement", "error", SearchPath, SearchValuePath, source?.GetType().Name);
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(SearchValuePath))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#25; SearchValuePath is empty. If this is intentional, please use XmlGetValue instead.", "error", SearchPath, SearchValuePath);
                return string.Empty;
            }

            MethodResult<string> searchValue = xElement.GetXPathValue(SearchValuePath);
            if (searchValue.IsValid && string.IsNullOrWhiteSpace(searchValue.Value))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#14; SearchPath resulted in empty string", "warning", SearchPath, SearchValuePath, source);
                return string.Empty;
            }

            if (!searchValue.IsValid)
                return string.Empty;

            string actualPath = string.IsNullOrWhiteSpace(searchValue.Value) ? SearchPath : SearchPath.Replace("{{searchValue}}", searchValue.Value);
            MethodResult<string> result = xElement.GetXPathValue(actualPath);
            if(result.IsValid && string.IsNullOrWhiteSpace(result.Value))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#15; ActualPath resulted in no items", "warning", actualPath, SearchPath, SearchValuePath, source);
                return string.Empty;
            }
            return result.Value;
        }
    }
}