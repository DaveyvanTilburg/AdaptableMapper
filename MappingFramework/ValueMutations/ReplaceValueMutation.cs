using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;
using MappingFramework.ValueMutations.Traversals;
using MappingFramework.Visitors;

namespace MappingFramework.ValueMutations
{
    [ContentType(ContentType.Any)]
    public sealed class ReplaceValueMutation : ValueMutation, ResolvableByTypeId, IVisitable
    {
        public const string _typeId = "ff9b3844-f3a9-4339-9fb9-d41133506391";
        public string TypeId => _typeId;

        public ReplaceValueMutation() { }
        public ReplaceValueMutation(GetValueStringTraversal getValueStringTraversal, GetValueTraversal getValueTraversal)
        {
            GetValueStringTraversal = getValueStringTraversal;
            GetValueTraversal = getValueTraversal;
        }

        public GetValueStringTraversal GetValueStringTraversal { get; set; }
        public GetValueTraversal GetValueTraversal { get; set; }

        public string Mutate(Context context, string value)
        {
            string valueToReplace = GetValueStringTraversal.GetValue(context, value);
            if (string.IsNullOrEmpty(valueToReplace))
                return value;

            string newValue = GetValueTraversal.GetValue(context);
            if (string.IsNullOrEmpty(newValue))
                return value;

            string result = value.Replace(valueToReplace, newValue);
            return result;
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(GetValueTraversal);
        }
    }
}