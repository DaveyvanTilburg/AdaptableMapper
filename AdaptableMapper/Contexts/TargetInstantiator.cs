namespace AdaptableMapper.Contexts
{
    public interface TargetInstantiator
    {
        object Create(object targetInstantiationMaterial);
    }
}