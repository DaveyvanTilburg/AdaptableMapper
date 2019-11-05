﻿using Newtonsoft.Json.Linq;
using System;

namespace AdaptableMapper.Json
{
    public sealed class JsonObjectConverter : ObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is string input))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#12; Source is not of expected type String", "error", source?.GetType().Name);
                return string.Empty;
            }

            JToken jToken;
            try
            {
                jToken = JToken.Parse(input);
            }
            catch (Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#13; Source could not be parsed to JToken", "error", source, exception.GetType().Name, exception.Message);
                jToken = new JObject();
            }

            return jToken;
        }
    }
}