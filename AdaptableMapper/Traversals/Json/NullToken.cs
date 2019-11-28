using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Traversals.Json
{
    internal class NullToken : JObject
    {
        public override JTokenType Type => JTokenType.Null;
    }
}