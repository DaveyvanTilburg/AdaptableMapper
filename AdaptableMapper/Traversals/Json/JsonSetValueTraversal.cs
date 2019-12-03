using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableMapper.Traversals.Json
{
    public sealed class JsonSetValueTraversal : SetMutableValueTraversal
    {
        public JsonSetValueTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        protected override void SetValueImplementation(object target, string value)
        {
            if (!(target is JToken jToken))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#18; target is not of expected type jToken", "error", target?.GetType().Name);
                return;
            }

            IReadOnlyCollection<JToken> jTokens = jToken.TraverseAll(Path).ToList();

            if (!jTokens.Any())
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#30; Path resulted in no targets to set value to", "warning", Path);
                return;
            }

            foreach (JToken jTokenTarget in jTokens.Where(t => t.Type != JTokenType.Null))
            {
                if (jTokenTarget is JValue jTokenTargetValue)
                    jTokenTargetValue.Value = value;
                else
                    Process.ProcessObservable.GetInstance().Raise("JSON#19; result of traversal is not of expected type JValue", "error", jTokenTarget?.GetType().Name);
            }
        }
    }
}