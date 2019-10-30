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
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type JValue");
                return;
            }

            jValue.Value = value;
        }
    }
}