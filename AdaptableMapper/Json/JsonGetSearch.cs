using AdaptableMapper.Traversals;
using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    public sealed class JsonGetSearch : GetValueTraversal
    {
        public JsonGetSearch(string path, string searchPath)
        {
            Path = path;
            SearchPath = searchPath;
        }

        public string Path { get; set; }
        public string SearchPath { get; set; }

        public string GetValue(object source)
        {
            if (!(source is JToken jToken))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type JToken");
                return string.Empty;
            }

            string searchValue = null;
            if (!string.IsNullOrWhiteSpace(SearchPath))
            {
                searchValue = jToken.SelectToken(SearchPath).ToString();
                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    Errors.ErrorObservable.GetInstance().Raise("SearchPath resulted in empty string");
                    return string.Empty;
                }
            }

            string actualPath = string.IsNullOrWhiteSpace(searchValue) ? Path : Path.Replace("{{searchValue}}", searchValue);

            JToken value = jToken.Traverse(actualPath);
            if (value == null)
            {
                Errors.ErrorObservable.GetInstance().Raise($"Search resulted in no result, path : {Path}; SearchPath : {SearchPath}; ActualPath : {actualPath}");
                return string.Empty;
            }

            string result = value.Value<string>();
            return result;
        }
    }
}