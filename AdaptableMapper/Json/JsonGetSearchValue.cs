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
                Process.ProcessObservable.GetInstance().Raise("JSON#5; source is not of expected type JToken", "error", SearchPath, SearchValuePath, source?.GetType().Name);
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(SearchValuePath))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#31; SearchValuePath is empty. If this is intentional, please use JsonGetValue instead.", "error", SearchPath, SearchValuePath);
                return string.Empty;
            }

            string searchValue = jToken.TryTraversalGetValue(SearchValuePath);
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#7; SearchPath resulted in empty string", "warning", SearchPath, SearchValuePath, source);
                return string.Empty;
            }

            string actualPath = string.IsNullOrWhiteSpace(searchValue) ? SearchPath : SearchPath.Replace("{{searchValue}}", searchValue);

            string result = jToken.TryTraversalGetValue(actualPath);
            if (string.IsNullOrWhiteSpace(result))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#8; ActualPath resulted in no items", "warning", actualPath, SearchPath, SearchValuePath, source);
                return string.Empty;
            }

            return result;
        }
    }
}