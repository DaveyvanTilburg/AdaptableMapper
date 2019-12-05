using System.Collections.Generic;
using System.Linq;
using AdaptableMapper.Configuration;

namespace AdaptableMapper.ValueMutations
{
    public class ListOfValueMutations : ValueMutation
    {
        public List<ValueMutation> ValueMutations { get; set; }

        public ListOfValueMutations()
            => ValueMutations = new List<ValueMutation>();

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