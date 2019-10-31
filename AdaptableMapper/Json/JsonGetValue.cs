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
                Errors.ErrorObservable.GetInstance().Raise("JSON#10; Source is not of expected type JToken", Path, source);
                return string.Empty;
            }

            JToken result = jToken.Traverse(Path);
            if (result == null)
            {
                Errors.ErrorObservable.GetInstance().Raise("JSON#11; Path resulted in no items", Path);
                return string.Empty;
            }

            return result.Value<string>();
        }
    }
}