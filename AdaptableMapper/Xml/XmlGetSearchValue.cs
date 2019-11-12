using AdaptableMapper.Traversals;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlGetSearchValue : GetValueTraversal
    {
        public XmlGetSearchValue(string searchPath, string searchValuePath)
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

            string searchValue = xElement.GetXPathValue(SearchValuePath);
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#14; SearchPath resulted in empty string", "warning", SearchPath, SearchValuePath, source);
                return string.Empty;
            }

            string actualPath = string.IsNullOrWhiteSpace(searchValue) ? SearchPath : SearchPath.Replace("{{searchValue}}", searchValue);
            string searchResult = xElement.GetXPathValue(actualPath);
            if(string.IsNullOrWhiteSpace(searchResult))
            {
                Process.ProcessObservable.GetInstance().Raise("XML#15; ActualPath resulted in no items", "warning", actualPath, SearchPath, SearchValuePath, source);
                return string.Empty;
            }
            return searchResult;
        }
    }
}