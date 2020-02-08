using Newtonsoft.Json.Linq;

namespace MappingFramework.Traversals.Json
{
    public sealed class NullToken : JObject
    {
        public override JTokenType Type => JTokenType.Null;
    }
}