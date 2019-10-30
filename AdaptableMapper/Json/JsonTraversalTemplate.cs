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
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type jToken");
                return new JObject();
            }

            JToken result = jToken.SelectToken(Path);
            if (result == null)
            {
                Errors.ErrorObservable.GetInstance().Raise($"Path {Path} resulted in no jTokens");
                return string.Empty;
            }

            result.Remove();

            return result;
        }
    }
}