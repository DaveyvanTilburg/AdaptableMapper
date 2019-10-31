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
                Errors.ErrorObservable.GetInstance().Raise("JSON#21; target is not of expected type jToken", Path, target);
                return new JObject();
            }

            JToken result = jToken.Traverse(Path);
            if (result == null)
            {
                Errors.ErrorObservable.GetInstance().Raise("JSON#22; Path resulted in no items", Path, jToken);
                return string.Empty;
            }

            return result;
        }
    }
}