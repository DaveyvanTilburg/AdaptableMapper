using System;
using System.Collections.Generic;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;

namespace MappingFramework.Languages.Dictionary.Traversals
{
    [ContentType(ContentType.Dictionary)]
    public sealed class DictionarySetValueTraversal : SetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "0ea332ca-fe05-4cad-99f1-9cd1eb54be2e";
        public string TypeId => _typeId;

        public DictionarySetValueTraversal() { }

        public DictionarySetValueTraversal(GetValueTraversal key)
        {
            Key = key;
        }

        public DictionarySetValueTraversal(GetValueTraversal key, DictionaryValueTypes dictionaryValueTypes)
        {
            Key = key;
            DictionaryValueType = dictionaryValueTypes;
        }

        public GetValueTraversal Key { get; set; }
        public DictionaryValueTypes DictionaryValueType { get; set; }


        public void SetValue(Context context, string value)
        {
            IDictionary<string, object> dictionary = (IDictionary<string, object>)context.Target;

            string key = Key.GetValue(context);
            if (string.IsNullOrWhiteSpace(key))
            {
                context.PropertyIsEmpty(this, nameof(Key));
                return;
            }

            switch (DictionaryValueType)
            {
                case DictionaryValueTypes.String:
                    dictionary[key] = value;
                    break;
                case DictionaryValueTypes.Integer:
                    if (!int.TryParse(value, out int integerValue))
                        context.OperationFailed(this, new Exception($"Value: '{value}' can not be parsed to an integer"));
                    else
                        dictionary[key] = integerValue;
                    break;
            }
        }
    }
}