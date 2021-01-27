using System.Collections.Generic;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;

namespace MappingFramework.Languages.Dictionary.Configuration
{
    [ContentType(ContentType.Dictionary)]
    public sealed class DictionaryTargetCreator : TargetCreator, ResolvableByTypeId
    {
        public const string _typeId = "e8a1efee-981d-443d-b363-4f1796491fab";
        public string TypeId => _typeId;

        public DictionaryTargetCreator() { }


        public object Create(Context context, object source)
        {
            if (source is IDictionary<string, object> dictionary)
                return dictionary;

            return new EasyAccessDictionary();
        }
    }
}