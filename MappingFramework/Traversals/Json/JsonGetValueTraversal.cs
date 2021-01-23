using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using Newtonsoft.Json.Linq;

namespace MappingFramework.Traversals.Json
{
    [ContentType(ContentType.Json)]
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
            JToken jToken = (JToken)context.Source;

            MethodResult<string> result = jToken.TryTraversalGetValue(Path, context);
            if (!result.IsValid)
                return string.Empty;

            return result.Value;
        }
    }
}