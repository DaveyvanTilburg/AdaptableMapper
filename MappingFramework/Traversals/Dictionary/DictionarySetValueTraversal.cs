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
                Process.ProcessObservable.GetInstance().Raise("DictionarySetValueTraversal#1; Key is not set", "error", Key, context.Target?.GetType().Name);
                return;
            }

            if (!(context.Target is IDictionary<string, object> dictionary))
            {
                Process.ProcessObservable.GetInstance().Raise("DictionarySetValueTraversal#2; target is not of expected type IDictionary<string, object>", "error", Key, context.Target?.GetType().Name);
                return;
            }

            switch (DictionaryValueType)
            {
                case DictionaryValueTypes.String:
                    dictionary[Key] = value;
                    break;
                case DictionaryValueTypes.Integer:
                    if (!int.TryParse(value, out int integerValue))
                        Process.ProcessObservable.GetInstance().Raise($"DictionarySetValueTraversal#3; value: {value} is not parsable to requested type: {DictionaryValueType}", "warning", Key, context.Target?.GetType().Name);
                    else
                        dictionary[Key] = integerValue;
                    break;
            }
        }
    }
}