using AdaptableMapper.Configuration;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Conditions
{
    public sealed class NotEmptyCondition : Condition, SerializableByTypeId
    {
        public const string _typeId = "63912c96-a37a-4888-b051-e226e383c652";
        public string TypeId => _typeId;

        public NotEmptyCondition() { }
        public NotEmptyCondition(GetValueTraversal getValueTraversal)
            => GetValueTraversal = getValueTraversal;

        public GetValueTraversal GetValueTraversal { get; set; }

        public bool Validate(Context context)
        {
            string value = GetValueTraversal.GetValue(context);

            bool result = !string.IsNullOrWhiteSpace(value);
            return result;
        }
    }
}