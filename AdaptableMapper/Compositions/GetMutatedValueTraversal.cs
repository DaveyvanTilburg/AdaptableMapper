using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using AdaptableMapper.Traversals;
using AdaptableMapper.ValueMutations;

namespace AdaptableMapper.Compositions
{
    public class GetMutatedValueTraversal : GetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "377fbe9e-a0c4-4b36-b3ed-ee1f92112047";
        public string TypeId => _typeId;

        public GetMutatedValueTraversal() { }
        public GetMutatedValueTraversal(GetValueTraversal getValueTraversal, ValueMutation valueMutation)
        {
            GetValueTraversal = getValueTraversal;
            ValueMutation = valueMutation;
        }

        public GetValueTraversal GetValueTraversal { get; set; }
        public ValueMutation ValueMutation { get; set; }

        public string GetValue(Context context)
        {
            if (!Validate())
                return string.Empty;

            string value = GetValueTraversal.GetValue(context);
            string result = ValueMutation.Mutate(context, value);

            return result;
        }

        private bool Validate()
        {
            bool result = true;

            if (GetValueTraversal == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"GetMutatedValueTraversal#1; {nameof(GetValueTraversal)} cannot be null", "error");
                result = false;
            }

            if (ValueMutation == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"GetMutatedValueTraversal#2; {nameof(ValueMutation)} cannot be null", "error");
                result = false;
            }

            return result;
        }
    }
}