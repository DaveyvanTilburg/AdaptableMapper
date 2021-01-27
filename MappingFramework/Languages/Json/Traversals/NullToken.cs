using Newtonsoft.Json.Linq;

namespace MappingFramework.Languages.Json.Traversals
{
    public sealed class NullToken : JObject
    {
        public override JTokenType Type => JTokenType.Null;
    }
}