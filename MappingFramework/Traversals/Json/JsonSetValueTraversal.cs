using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using MappingFramework.Configuration;
using MappingFramework.Converters;

namespace MappingFramework.Traversals.Json
{
    public sealed class JsonSetValueTraversal : SetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "4339b4c9-cdb9-44ab-bebc-b9d3bd2a5287";
        public string TypeId => _typeId;

        public JsonSetValueTraversal() { }
        public JsonSetValueTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public void SetValue(Context context, MappingCaches mappingCaches, string value)
        {
            if (!(context.Target is JToken jToken))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#18; target is not of expected type jToken", "error", context.Target?.GetType().Name);
                return;
            }

            IReadOnlyCollection<JToken> jTokens = jToken.TraverseAll(Path).ToList();

            if (!jTokens.Any())
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#30; Path resulted in no targets to set value to", "warning", Path);
                return;
            }

            foreach (JToken jTokenTarget in jTokens.Where(t => !(t.Type == JTokenType.Null && t.Parent == null)))
            {
                if (jTokenTarget is JValue jTokenTargetValue)
                    jTokenTargetValue.Value = value;
                else
                    Process.ProcessObservable.GetInstance().Raise("JSON#19; result of traversal is not of expected type JValue", "error", jTokenTarget?.GetType().Name);
            }
        }
    }
}