using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Traversals.Json
{
    public sealed class JsonGetValueTraversal : GetValueTraversal, GetValueTraversalPathProperty, ResolvableByTypeId
    {
        public const string _typeId = "fb65549b-5593-42f9-9ffe-3eddf10913e6";
        public string TypeId => _typeId;

        public JsonGetValueTraversal() { }
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
            if (!result.IsValid)
                return string.Empty;

            return result.Value;
        }
    }
}