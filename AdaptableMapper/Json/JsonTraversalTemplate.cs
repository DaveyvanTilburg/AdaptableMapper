using AdaptableMapper.Traversals;
using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    public sealed class JsonTraversalTemplate : TraversalToGetTemplate
    {
        public JsonTraversalTemplate(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public object Traverse(object target)
        {
            if (!(target is JToken jToken))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#23; target is not of expected type jToken", "error", Path, target?.GetType().Name);
                return new JObject();
            }

            JToken result = jToken.Traverse(Path);
            if (result == null)
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#24; Path resulted in no items", "warning", Path, target);
                return string.Empty;
            }

            result.Remove();

            return result;
        }
    }
}