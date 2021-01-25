using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;

namespace MappingFramework.Traversals.Json
{
    [ContentType(ContentType.Json)]
    public sealed class JsonGetListValueTraversal : GetListSearchPathValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "5d1df2c9-6af6-45a5-81c4-c2885de4b5c1";
        public string TypeId => _typeId;

        public JsonGetListValueTraversal() { }
        public JsonGetListValueTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public MethodResult<IEnumerable<object>> GetValues(Context context)
        {
            JToken jToken = (JToken)context.Source;

            IEnumerable<JToken> jTokens = jToken.TraverseAll(Path, context).ToList();
            if (!jTokens.Any())
            {
                context.NavigationResultIsEmpty(Path);
                return new NullMethodResult<IEnumerable<object>>();
            }

            return new MethodResult<IEnumerable<object>>(jTokens);
        }

        string GetValueTraversalPath.Path() => Path;
        void GetValueTraversalPath.Path(string path) => Path = path;
    }
}