using AdaptableMapper.Contexts;

namespace AdaptableMapper
{
    public interface MappingScope
    {
        void Traverse(Context context);
    }
}