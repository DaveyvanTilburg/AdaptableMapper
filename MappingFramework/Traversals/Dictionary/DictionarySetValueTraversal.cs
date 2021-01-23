using System;
using System.Collections.Generic;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;

namespace MappingFramework.Traversals.Dictionary
{
    [ContentType(ContentType.Dictionary)]
    public sealed class DictionarySetValueTraversal : SetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "0ea332ca-fe05-4cad-99f1-9cd1eb54be2e";
        public string TypeId => _typeId;

        public DictionarySetValueTraversal() { }

        public DictionarySetValueTraversal(string key)
        {
            Key = key;
        }

        public DictionarySetValueTraversal(string key, DictionaryValueTypes dictionaryValueTypes)
        {
            Key = key;
            DictionaryValueType = dictionaryValueTypes;
        }

        public string Key { get; set; }
        public DictionaryValueTypes DictionaryValueType { get; set; }


        public void SetValue(Context context, MappingCaches mappingCaches, string value)
        {
            if (string.IsNullOrWhiteSpace(Key))
            {
                context.PropertyIsEmpty(this, nameof(Key));
                return;
            }

            IDictionary<string, object> dictionary = (IDictionary<string, object>)context.Target;

            switch (DictionaryValueType)
            {
                case DictionaryValueTypes.String:
                    dictionary[Key] = value;
                    break;
                case DictionaryValueTypes.Integer:
                    if (!int.TryParse(value, out int integerValue))
                        context.OperationFailed(this, new Exception($"Value: '{value}' can not be parsed to an integer"));
                    else
                        dictionary[Key] = integerValue;
                    break;
            }
        }
    }
}