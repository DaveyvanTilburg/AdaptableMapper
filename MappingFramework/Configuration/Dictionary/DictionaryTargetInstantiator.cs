using System.Collections.Generic;
using MappingFramework.Converters;
using MappingFramework.Dictionary;

namespace MappingFramework.Configuration.Dictionary
{
    public sealed class DictionaryTargetInstantiator : TargetInstantiator, ResolvableByTypeId
    {
        public const string _typeId = "e8a1efee-981d-443d-b363-4f1796491fab";
        public string TypeId => _typeId;

        public DictionaryTargetInstantiator() { }


        public object Create(object source)
        {
            if (source is IDictionary<string, object> dictionary)
                return dictionary;

            return new EasyAccessDictionary();
        }
    }
}