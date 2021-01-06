﻿using MappingFramework.Converters;
using Newtonsoft.Json;

namespace MappingFramework.Configuration
{
    [JsonConverter(typeof(JsonTypeIdBasedConverter))]
    public interface ObjectConverter
    {
        object Convert(object source);
    }
}