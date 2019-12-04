using AdaptableMapper.Configuration;
using AdaptableMapper.ValueMutations.Traversals;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableMapper.ValueMutations
{
    public class DictionaryReplaceValueMutation : ValueMutation
    {
        public GetValueStringTraveral GetValueStringTraversal { get; set; }
        public Dictionary<string, string> ReplaceValues { get; set; }

        public DictionaryReplaceValueMutation(Dictionary<string, string> replaceValues)
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
            foreach (KeyValuePair<string, string> replaceValue in ReplaceValues)
                newValue = newValue.Replace(replaceValue.Key, replaceValue.Value);

            string result = value.Replace(valueToMutate, newValue);
            return result;
        }
    }
}