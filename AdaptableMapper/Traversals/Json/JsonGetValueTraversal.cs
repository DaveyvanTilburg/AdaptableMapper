using AdaptableMapper.Configuration;
using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Traversals.Json
{
    public sealed class JsonGetValueTraversal : GetValueTraversal
    {
        public JsonGetValueTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public string GetValue(Context context)
        {
            if (!(context.Source is JToken jToken))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#10; Source is not of expected type JToken", "error", Path, context.Source?.GetType().Name);
                return string.Empty;
            }

            MethodResult<string> result = jToken.TryTraversalGetValue(Path);
            if (result.IsValid && string.IsNullOrWhiteSpace(result.Value))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#11; Path resulted in no items", "warning", Path);
                return string.Empty;
            }

            return result.Value;
        }
    }
}