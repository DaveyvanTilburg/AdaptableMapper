using AdaptableMapper.Traversals;
using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    public sealed class JsonSetThisValue : SetValueTraversal
    {
        public void SetValue(object target, string value)
        {
            if (!(target is JValue jValue))
            {
                Errors.ProcessObservable.GetInstance().Raise("JSON#17; Target is not of expected type JValue", "error", target?.GetType()?.Name);
                return;
            }

            jValue.Value = value;
        }
    }
}