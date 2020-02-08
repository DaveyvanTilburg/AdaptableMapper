using System.Collections.Generic;
using MappingFramework.Configuration;
using MappingFramework.Converters;
using Newtonsoft.Json;

namespace MappingFramework.Traversals
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface GetListValueTraversal
    {
        MethodResult<IEnumerable<object>> GetValues(Context context);
    }
}