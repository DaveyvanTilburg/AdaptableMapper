using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;
using MappingFramework.ValueMutations;
using MappingFramework.Visitors;

namespace MappingFramework.Compositions
{
    [ContentType(ContentType.Any)]
    public class GetMutatedValueTraversal : GetValueTraversal, ResolvableByTypeId, IVisitable
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
            string value = GetValueTraversal.GetValue(context);
            string result = ValueMutation.Mutate(context, value);

            return result;
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(GetValueTraversal);
            visitor.Visit(ValueMutation);
        }
    }
}