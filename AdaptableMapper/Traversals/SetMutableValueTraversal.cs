using AdaptableMapper.Configuration;
using AdaptableMapper.ValueMutations;

namespace AdaptableMapper.Traversals
{
    public abstract class SetMutableValueTraversal : SetValueTraversal
    {
        public ValueMutation ValueMutation { get; set; }

        protected abstract void SetValueImplementation(Context context, MappingCaches mappingCaches, string value);

        public void SetValue(Context context, MappingCaches mappingCaches, string value)
        {
            string formattedValue = ValueMutation?.Mutate(context, value) ?? value;

            SetValueImplementation(context, mappingCaches, formattedValue);
        }
    }
}