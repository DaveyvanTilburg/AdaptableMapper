using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using AdaptableMapper.Traversals;
using AdaptableMapper.ValueMutations;

namespace AdaptableMapper.Compositions
{
    public class SetMutatedValueTraversal : SetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "1dfdbb1e-addc-4fb6-a197-e0c276f4fba0";
        public string TypeId => _typeId;

        public SetMutatedValueTraversal() { }
        public SetMutatedValueTraversal(SetValueTraversal setValueTraversal, ValueMutation valueMutation)
        {
            SetValueTraversal = setValueTraversal;
            ValueMutation = valueMutation;
        }

        public SetValueTraversal SetValueTraversal { get; set; }
        public ValueMutation ValueMutation { get; set; }

        public void SetValue(Context context, MappingCaches mappingCaches, string value)
        {
            if (!Validate())
                return;

            string mutatedValue = ValueMutation.Mutate(context, value);
            SetValueTraversal.SetValue(context, mappingCaches, mutatedValue);
        }

        private bool Validate()
        {
            bool result = true;

            if (SetValueTraversal == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"SetMutatedValueTraversal#1; {nameof(SetValueTraversal)} cannot be null", "error");
                result = false;
            }

            if (ValueMutation == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"SetMutatedValueTraversal#2; {nameof(ValueMutation)} cannot be null", "error");
                result = false;
            }

            return result;
        }
    }
}