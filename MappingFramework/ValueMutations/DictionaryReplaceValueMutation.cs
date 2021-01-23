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
            string valueToMutate = GetValueStringTraversal?.GetValue(context, value) ?? value;
            if (string.IsNullOrEmpty(valueToMutate))
                return value;

            string newValue = ReplaceValues.FirstOrDefault(r => r.ValueToReplace.Equals(valueToMutate))?.NewValue ?? valueToMutate;

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