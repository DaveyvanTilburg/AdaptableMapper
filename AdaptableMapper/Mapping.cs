using AdaptableMapper.Contexts;
using AdaptableMapper.Traversals;

namespace AdaptableMapper
{
    public sealed class Mapping
    {
        public GetValueTraversal GetValueTraversal { get; set; }
        public SetValueTraversal SetValueTraversal { get; set; }

        public Mapping(
            GetValueTraversal getValueTraversal, 
            SetValueTraversal setValueTraversal)
        {
            GetValueTraversal = getValueTraversal;
            SetValueTraversal = setValueTraversal;
        }

        internal void Map(Context context)
        {
            string value = GetValueTraversal.GetValue(context.Source);

            SetValueTraversal.SetValue(context.Target, value);
        }
    }
}