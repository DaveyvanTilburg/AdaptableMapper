using System.Collections.Generic;
using System.Linq;
using AdaptableMapper.Configuration;

namespace AdaptableMapper.ValueMutations
{
    public sealed class ListOfValueMutations : ValueMutation, SerializableByTypeId
    {
        public const string _typeId = "6b40f249-b509-482a-b98a-15616e6526f2";
        public string TypeId => _typeId;

        public ListOfValueMutations()
            => ValueMutations = new List<ValueMutation>();

        public ListOfValueMutations(IEnumerable<ValueMutation> valueMutations)
            => valueMutations = new List<ValueMutation>(valueMutations);

        public List<ValueMutation> ValueMutations { get; set; }

        public string Mutate(Context context, string value)
        {
            var result = value;

            if (!Validate())
                return result;

            foreach(ValueMutation valueMutation in ValueMutations)
                result = valueMutation.Mutate(context, result);

            return result;
        }

        private bool Validate()
        {
            bool result = true;

            if ((ValueMutations?.Any() ?? false) == false)
            {
                Process.ProcessObservable.GetInstance().Raise($"ListOfValueMutations#1; {nameof(ValueMutations)} is empty", "error");
                result = false;
            }

            return result;
        }
    }
}