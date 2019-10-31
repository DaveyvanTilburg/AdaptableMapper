using AdaptableMapper.Traversals;
using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    public sealed class JsonGetSearchValue : GetValueTraversal
    {
        public JsonGetSearchValue(string searchPath, string searchValuePath)
        {
            SearchPath = searchPath;
            SearchValuePath = searchValuePath;
        }

        public string SearchPath { get; set; }
        public string SearchValuePath { get; set; }

        public string GetValue(object source)
        {
            if (!(source is JToken jToken))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type JToken");
                return string.Empty;
            }

            string searchValue = null;
            if (!string.IsNullOrWhiteSpace(SearchValuePath))
            {
                JToken searchToken = jToken.Traverse(SearchValuePath);

                if(searchToken == null)
                {
                    Errors.ErrorObservable.GetInstance().Raise("Json GetSearchValue searchPath did not result in a token");
                    return string.Empty;
                }

                searchValue = searchToken.Value<string>();
                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    Errors.ErrorObservable.GetInstance().Raise("SearchPath resulted in empty string");
                    return string.Empty;
                }
            }

            string actualPath = string.IsNullOrWhiteSpace(searchValue) ? SearchPath : SearchPath.Replace("{{searchValue}}", searchValue);

            JToken searchResult = jToken.Traverse(actualPath);
            if (searchResult == null)
            {
                Errors.ErrorObservable.GetInstance().Raise($"Search resulted in no result, path : {SearchPath}; SearchPath : {SearchValuePath}; ActualPath : {actualPath}");
                return string.Empty;
            }

            string result = searchResult.Value<string>();
            return result;
        }
    }
}