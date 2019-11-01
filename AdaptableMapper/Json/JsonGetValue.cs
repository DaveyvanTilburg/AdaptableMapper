using AdaptableMapper.Traversals;
using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    public sealed class JsonGetValue : GetValueTraversal
    {
        public JsonGetValue(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public string GetValue(object source)
        {
            if (!(source is JToken jToken))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#10; Source is not of expected type JToken", "error", Path, source?.GetType().Name);
                return string.Empty;
            }

            JToken result = jToken.Traverse(Path);
            if (result == null)
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#11; Path resulted in no items", "warning", Path);
                return string.Empty;
            }

            return result.Value<string>();
        }
    }
}