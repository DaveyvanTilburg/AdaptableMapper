using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;

namespace MappingFramework.Traversals.Json
{
    [ContentType(ContentType.Json)]
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
            JToken jToken = (JToken)context.Target;
            IReadOnlyCollection<JToken> jTokens = jToken.TraverseAll(Path, context).ToList();

            if (!jTokens.Any())
            {
                context.NavigationResultIsEmpty(Path);
                return;
            }

            foreach (JToken jTokenTarget in jTokens.Where(t => !(t.Type == JTokenType.Null && t.Parent == null)))
            {
                if (jTokenTarget is JValue jTokenTargetValue)
                    jTokenTargetValue.Value = value;
                else
                    context.NavigationFailed(Path);
            }
        }
    }
}