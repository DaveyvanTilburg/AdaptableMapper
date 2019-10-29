using AdaptableMapper.Contexts;
using AdaptableMapper.Traversals;

namespace AdaptableMapper
{
    public sealed class Mapping
    {
        public GetValueTraversal GetTraversion { get; set; }
        public SetValueTraversal SetTraversion { get; set; }

        public Mapping(
            GetValueTraversal getTraversion, 
            SetValueTraversal setTraversion)
        {
            GetTraversion = getTraversion;
            SetTraversion = setTraversion;
        }

        internal void Map(Context context)
        {
            string value = GetTraversion.GetValue(context.Source);

            SetTraversion.SetValue(context.Target, value);
        }
    }
}