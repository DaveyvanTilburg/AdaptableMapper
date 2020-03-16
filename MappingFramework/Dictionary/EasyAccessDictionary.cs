using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MappingFramework.Dictionary
{
    [Serializable]
    public class EasyAccessDictionary : Dictionary<string, object>
    {
        public EasyAccessDictionary() { }
        protected EasyAccessDictionary(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

        public T GetValueAs<T>(string key)
        {
            if (!this.ContainsKey(key))
                return default;

            return (T)this[key];
        }
    }
}