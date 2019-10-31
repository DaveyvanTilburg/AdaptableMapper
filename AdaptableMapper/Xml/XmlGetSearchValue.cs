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
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type XElement");
                return string.Empty;
            }

            string searchValue = null;
            if (!string.IsNullOrWhiteSpace(SearchValuePath))
            {
                searchValue = xElement.GetXPathValues(SearchValuePath).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    Errors.ErrorObservable.GetInstance().Raise("SearchPath resulted in empty string");
                    return string.Empty;
                }
            }

            string actualPath = string.IsNullOrWhiteSpace(searchValue) ? SearchPath : SearchPath.Replace("{{searchValue}}", searchValue);

            IEnumerable<string> searchResults = xElement.GetXPathValues(actualPath);
            if(!searchResults.Any())
            {
                Errors.ErrorObservable.GetInstance().Raise($"Search resulted in no results, path : {SearchPath}; SearchPath : {SearchValuePath}; ActualPath : {actualPath}");
                return string.Empty;
            }

            string searchResult = searchResults.FirstOrDefault();
            return searchResult;
        }
    }
}