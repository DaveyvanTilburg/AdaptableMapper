using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Traversals.Json
{
    public class NullToken : JObject
    {
        public override JTokenType Type => JTokenType.Null;
    }
}