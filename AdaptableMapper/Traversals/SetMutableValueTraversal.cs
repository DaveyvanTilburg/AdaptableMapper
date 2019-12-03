using AdaptableMapper.ValueMutations;

namespace AdaptableMapper.Traversals
{
    public abstract class SetMutableValueTraversal : SetValueTraversal
    {
        public ValueMutation ValueMutation { get; set; }

        protected abstract void SetValueImplementation(object target, string value);

        public void SetValue(object target, string value)
        {
            string formattedValue = ValueMutation?.Mutate(value) ?? value;

            SetValueImplementation(target, formattedValue);
        }
    }
}