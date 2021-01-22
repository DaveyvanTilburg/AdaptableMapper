using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.Traversals;
using MappingFramework.ValueMutations;
using MappingFramework.Visitors;

namespace MappingFramework.Compositions
{
    public class SetMutatedValueTraversal : SetValueTraversal, ResolvableByTypeId, IVisitable
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
            string mutatedValue = ValueMutation.Mutate(context, value);
            SetValueTraversal.SetValue(context, mappingCaches, mutatedValue);
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(SetValueTraversal);
            visitor.Visit(ValueMutation);
        }
    }
}