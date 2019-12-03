using System.Collections.Generic;
using System.Linq;

namespace AdaptableMapper.ValueMutations
{
    public class ListOfValueMutations : ValueMutation
    {
        private List<ValueMutation> ValueMutations { get; set; }

        public string Mutate(string source)
        {
            var result = source;

            if (!Validate())
                return result;

            foreach(ValueMutation valueMutation in ValueMutations)
                result = valueMutation.Mutate(result);

            return result;
        }

        private bool Validate()
        {
            bool result = true;

            if ((ValueMutations?.Any() ?? false) == false)
            {
                Process.ProcessObservable.GetInstance().Raise("ListOfValueMutations#1; ValueMutations is empty", "error");
                result = false;
            }

            return result;
        }
    }
}