using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    internal class NullObject : JObject
    {
        public override JTokenType Type => JTokenType.Null;
    }
}