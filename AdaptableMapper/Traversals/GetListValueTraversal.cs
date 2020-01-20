using System.Collections.Generic;
using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using Newtonsoft.Json;

namespace AdaptableMapper.Traversals
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface GetListValueTraversal
    {
        MethodResult<IEnumerable<object>> GetValues(Context context);
    }
}