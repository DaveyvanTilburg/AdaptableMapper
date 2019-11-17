using AdaptableMapper.Traversals;
using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    public sealed class JsonTraversalFirstInListTemplate : TraversalToGetTemplate
    {
        public object Traverse(object target)
        {
            if (!(target is JToken jToken))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#27; target is not of expected type jToken", "error", "[0]", target?.GetType().Name);
                return new JObject();
            }

            JToken result = jToken.Traverse("[0]");
            if (string.IsNullOrWhiteSpace(result.Path))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#28; Path resulted in no items", "warning", nameof(JsonTraversalFirstInListTemplate), target);
                return new JObject();
            }

            result.Remove();

            return result;
        }
    }
}