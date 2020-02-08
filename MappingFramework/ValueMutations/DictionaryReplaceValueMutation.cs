using MappingFramework.Configuration;
using MappingFramework.ValueMutations.Traversals;
using System.Collections.Generic;
using System.Linq;
using MappingFramework.Converters;

namespace MappingFramework.ValueMutations
{
    public sealed class DictionaryReplaceValueMutation : ValueMutation, ResolvableByTypeId
    {
        public const string _typeId = "1a18c1a7-1799-4a56-85d2-2e2090252945";
        public string TypeId => _typeId;

        public DictionaryReplaceValueMutation()
            => ReplaceValues = new List<ReplaceValue>();
        public DictionaryReplaceValueMutation(List<ReplaceValue> replaceValues)
            => ReplaceValues = replaceValues;

        public GetValueStringTraversal GetValueStringTraversal { get; set; }
        public List<ReplaceValue> ReplaceValues { get; set; }

        public string Mutate(Context context, string value)
        {
            if (!ReplaceValues.Any())
            {
                Process.ProcessObservable.GetInstance().Raise("DictionaryReplaceValueMutation#1; No replacement values set", "error");
                return value;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                Process.ProcessObservable.GetInstance().Raise("DictionaryReplaceValueMutation#2; cannot mutate an empty string", "warning");
                return value;
            }

            string valueToMutate = GetValueStringTraversal?.GetValue(value) ?? value;
            if (string.IsNullOrEmpty(valueToMutate))
                return value;

            string newValue = valueToMutate;
            foreach (ReplaceValue replaceValue in ReplaceValues)
                newValue = newValue.Replace(replaceValue.ValueToReplace, replaceValue.NewValue);

            string result = value.Replace(valueToMutate, newValue);
            return result;
        }

        public class ReplaceValue
        {
            public ReplaceValue() { }
            public ReplaceValue(string valueToReplace, string newValue)
            {
                ValueToReplace = valueToReplace;
                NewValue = newValue;
            }

            public string ValueToReplace { get; set; }
            public string NewValue { get; set; }
        }
    }
}