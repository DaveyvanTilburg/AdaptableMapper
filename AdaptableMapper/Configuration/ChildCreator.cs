using AdaptableMapper.Traversals;

namespace AdaptableMapper.Configuration
{
    public interface ChildCreator
    {
        object CreateChild(Template template);
        void AddToParent(Template template, object newChild);
    }
}