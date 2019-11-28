namespace AdaptableMapper.Configuration
{
    public interface TargetInstantiator
    {
        object Create(object source);
    }
}