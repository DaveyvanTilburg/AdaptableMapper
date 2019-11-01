﻿using AdaptableMapper.Traversals;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AdaptableMapper.Json
{
    public sealed class JsonSetValue : SetValueTraversal
    {
        public JsonSetValue(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public void SetValue(object target, string value)
        {
            if (!(target is JToken jToken))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#18; target is not of expected type jToken", "error", target?.GetType().Name);
                return;
            }

            IEnumerable<JToken> jTokens = jToken.TraverseAll(Path);

            foreach(JToken jTokenTarget in jTokens)
            {
                if (jTokenTarget is JValue jTokenTargetValue)
                    jTokenTargetValue.Value = value;
                else
                    Process.ProcessObservable.GetInstance().Raise("JSON#19; result of traversal is not of expected type JValue", "error", jTokenTarget?.GetType().Name);
            }
        }
    }
}