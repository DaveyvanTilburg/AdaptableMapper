namespace AdaptableMapper
{
    public interface ChildCreator
    {
        object CreateChildOn(object parent, object template);
    }
}