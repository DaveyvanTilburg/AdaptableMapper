using AdaptableMapper.Contexts;

namespace AdaptableMapper.Traversals
{
    public sealed class Mapping
    {
        public GetTraversal GetTraversion { get; set; }
        public SetTraversal SetTraversion { get; set; }

        public Mapping(
            GetTraversal getTraversion, 
            SetTraversal setTraversion)
        {
            GetTraversion = getTraversion;
            SetTraversion = setTraversion;
        }

        public void Map(Context context)
        {
            string value = GetTraversion.GetValue(context.Source);

            SetTraversion.SetValue(context.Target, value);
        }
    }
}