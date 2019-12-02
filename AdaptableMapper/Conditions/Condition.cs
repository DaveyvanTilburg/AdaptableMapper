namespace AdaptableMapper.Conditions
{
    public interface Condition
    {
        bool Validate(object source);
    }
}