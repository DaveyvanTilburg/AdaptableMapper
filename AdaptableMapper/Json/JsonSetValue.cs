using AdaptableMapper.Traversals;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AdaptableMapper.Json
{
    public sealed class JsonSetValue : SetValueTraversal
    {
        public JsonSetValue(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public void SetValue(object target, string value)
        {
            if (!(target is JToken jToken))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type jToken");
                return;
            }

            IEnumerable<JToken> jTokens = jToken.TraverseAll(Path);

            foreach(JToken jTokenTarget in jTokens)
            {
                if (jTokenTarget is JValue jTokenTargetValue)
                    jTokenTargetValue.Value = value;
                else
                    Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type jValue");
            }
        }
    }
}