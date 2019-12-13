using AdaptableMapper.Configuration;

namespace AdaptableMapper.Traversals
{
    public interface SetValueTraversal
    {
        void SetValue(Context context, MappingCaches mappingCaches, string value);
    }
}