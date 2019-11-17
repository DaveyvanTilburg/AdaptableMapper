using AdaptableMapper.Traversals;
using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    public sealed class JsonTraversal : Traversal
    {
        public JsonTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public object Traverse(object target)
        {
            if (!(target is JToken jToken))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#21; target is not of expected type jToken", "error", Path, target?.GetType().Name);
                return new JObject();
            }

            JToken result = jToken.Traverse(Path);
            if (string.IsNullOrWhiteSpace(result.Path))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#22; Path resulted in no items", "warning", Path, jToken);
                return string.Empty;
            }

            return result;
        }
    }
}