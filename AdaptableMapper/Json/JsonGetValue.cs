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
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type JToken");
                return string.Empty;
            }

            JToken result = jToken.Traverse(Path);
            if (result == null)
            {
                Errors.ErrorObservable.GetInstance().Raise("Json GetValue did not result in a token");
                return string.Empty;
            }

            return result.Value<string>();
        }
    }
}