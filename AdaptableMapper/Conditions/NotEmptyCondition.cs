using AdaptableMapper.Configuration;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Conditions
{
    public sealed class NotEmptyCondition : Condition
    {
        public NotEmptyCondition(GetValueTraversal getValueTraversal)
        {
            GetValueTraversal = getValueTraversal;
        }

        public GetValueTraversal GetValueTraversal { get; set; }

        public bool Validate(Context context)
        {
            string value = GetValueTraversal.GetValue(context);

            bool result = !string.IsNullOrWhiteSpace(value);
            return result;
        }
    }
}