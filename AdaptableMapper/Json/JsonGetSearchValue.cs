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
                Errors.ErrorObservable.GetInstance().Raise("JSON#5; source is not of expected type JToken", SearchPath, SearchValuePath, source?.GetType()?.Name);
                return string.Empty;
            }

            string searchValue = null;
            if (!string.IsNullOrWhiteSpace(SearchValuePath))
            {
                JToken searchToken = jToken.Traverse(SearchValuePath);

                if(searchToken == null)
                {
                    Errors.ErrorObservable.GetInstance().Raise("JSON#6; SearchPath resulted in no items", SearchPath, SearchValuePath, source);
                    return string.Empty;
                }

                searchValue = searchToken.Value<string>();
                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    Errors.ErrorObservable.GetInstance().Raise("JSON#7; SearchPath resulted in empty string", SearchPath, SearchValuePath, source);
                    return string.Empty;
                }
            }

            string actualPath = string.IsNullOrWhiteSpace(searchValue) ? SearchPath : SearchPath.Replace("{{searchValue}}", searchValue);

            JToken searchResult = jToken.Traverse(actualPath);
            if (searchResult == null)
            {
                Errors.ErrorObservable.GetInstance().Raise("JSON#8; ActualPath resulted in no items", actualPath, SearchPath, SearchValuePath, source);
                return string.Empty;
            }

            string result = searchResult.Value<string>();
            return result;
        }
    }
}