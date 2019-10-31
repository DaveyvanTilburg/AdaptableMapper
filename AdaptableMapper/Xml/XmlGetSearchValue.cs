using AdaptableMapper.Traversals;
using System.Collections.Generic;
using System.Linq;
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
                Errors.ErrorObservable.GetInstance().Raise("XML#13; source is not of expected type XElement", SearchPath, SearchValuePath, source);
                return string.Empty;
            }

            string searchValue = null;
            if (!string.IsNullOrWhiteSpace(SearchValuePath))
            {
                searchValue = xElement.GetXPathValues(SearchValuePath).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    Errors.ErrorObservable.GetInstance().Raise("XML#14; SearchPath resulted in empty string", SearchPath, SearchValuePath, source);
                    return string.Empty;
                }
            }

            string actualPath = string.IsNullOrWhiteSpace(searchValue) ? SearchPath : SearchPath.Replace("{{searchValue}}", searchValue);

            IEnumerable<string> searchResults = xElement.GetXPathValues(actualPath);
            if(!searchResults.Any())
            {
                Errors.ErrorObservable.GetInstance().Raise("XML#15; ActualPath resulted in no items", actualPath, SearchPath, SearchValuePath, source);
                return string.Empty;
            }

            string searchResult = searchResults.FirstOrDefault();
            return searchResult;
        }
    }
}