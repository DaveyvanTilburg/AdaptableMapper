using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    internal class NullToken : JObject
    {
        public override JTokenType Type => JTokenType.Null;
    }
}