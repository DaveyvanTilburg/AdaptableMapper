using AdaptableMapper.Traversals;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AdaptableMapper.Xml
{
    public sealed class XmlGetSearch : GetValueTraversal
    {
        public XmlGetSearch(string path, string searchPath)
        {
            Path = path;
            SearchPath = searchPath;
        }

        public string Path { get; set; }
        public string SearchPath { get; set; }

        public string GetValue(object source)
        {
            if (!(source is XElement xElement))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type XElement");
                return string.Empty;
            }

            string searchValue = null;
            if (!string.IsNullOrWhiteSpace(SearchPath))
            {
                searchValue = xElement.GetXPathValues(SearchPath).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    Errors.ErrorObservable.GetInstance().Raise("SearchPath resulted in empty string");
                    return string.Empty;
                }
            }

            string actualPath = string.IsNullOrWhiteSpace(searchValue) ? Path : Path.Replace("{{searchValue}}", searchValue);

            IEnumerable<string> values = xElement.GetXPathValues(actualPath);
            if(!values.Any())
            {
                Errors.ErrorObservable.GetInstance().Raise($"Search resulted in no results, path : {Path}; SearchPath : {SearchPath}; ActualPath : {actualPath}");
                return string.Empty;
            }

            string value = values.FirstOrDefault();
            return value;
        }
    }
}