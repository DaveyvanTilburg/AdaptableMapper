using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;
using Newtonsoft.Json.Linq;

namespace MappingFramework.Languages.Json.Traversals
{
    [ContentType(ContentType.Json)]
    public sealed class JsonGetValueTraversal : GetSearchPathValueTraversal, ResolvableByTypeId
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

        string GetValueTraversalPath.Path() => Path;
        void GetValueTraversalPath.Path(string path) => Path = path;
    }
}