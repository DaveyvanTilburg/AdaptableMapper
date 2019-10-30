using AdaptableMapper.Traversals;
using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    public sealed class JsonGetThisValue : GetValueTraversal
    {
        public string GetValue(object source)
        {
            if (!(source is JToken jToken))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type JToken");
                return string.Empty;
            }

            return jToken.Value<string>();
        }
    }
}