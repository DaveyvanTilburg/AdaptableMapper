using Newtonsoft.Json.Linq;
using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;

namespace AdaptableMapper.Traversals.Json
{
    public sealed class JsonSetThisValueTraversal : SetMutableValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "5558248c-84c6-4247-b124-80577710e23f";
        public string TypeId => _typeId;

        public JsonSetThisValueTraversal() { }

        protected override void SetValueImplementation(Context context, MappingCaches mappingCaches, string value)
        {
            if (!(context.Target is JToken jToken))
            {
                Process.ProcessObservable.GetInstance().Raise("JsonSetThisValueTraversal#1; target is not of expected type jToken", "error", context.Target?.GetType().Name);
                return;
            }

            if (jToken is JValue jTokenTargetValue)
                jTokenTargetValue.Value = value;
            else
                Process.ProcessObservable.GetInstance().Raise("JsonSetThisValueTraversal#2; result of traversal is not of expected type JValue", "error", jToken?.GetType().Name);
        }
    }
}