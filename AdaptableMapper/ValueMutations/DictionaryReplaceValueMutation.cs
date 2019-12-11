using AdaptableMapper.Configuration;
using AdaptableMapper.ValueMutations.Traversals;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableMapper.ValueMutations
{
    public sealed class DictionaryReplaceValueMutation : ValueMutation
    {
        public GetValueStringTraversal GetValueStringTraversal { get; set; }
        public List<ReplaceValue> ReplaceValues { get; set; }

        public DictionaryReplaceValueMutation(List<ReplaceValue> replaceValues)
            => ReplaceValues = replaceValues;

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
            public string ValueToReplace { get; set; }
            public string NewValue { get; set; }
        }
    }
}