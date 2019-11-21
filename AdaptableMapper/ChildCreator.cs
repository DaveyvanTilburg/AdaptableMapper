using AdaptableMapper.Traversals;

namespace AdaptableMapper
{
    public interface ChildCreator
    {
        object CreateChild(Template template);
    }
}