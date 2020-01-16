using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Traversals.Json
{
    public sealed class JsonGetThisValueTraversal : GetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "3c82222f-84e3-4817-b6bf-541897b0c168";
        public string TypeId => _typeId;

        public JsonGetThisValueTraversal() { }

        public string GetValue(Context context)
        {
            if (!(context.Source is JToken jToken))
            {
                Process.ProcessObservable.GetInstance().Raise("JsonGetThisValueTraversal#1; Source is not of expected type JToken", "error", context.Source?.GetType().Name);
                return string.Empty;
            }

            string result = jToken.Value<string>();
            return result;
        }
    }
}