namespace AdaptableMapper.Model
{
    public sealed class NullModel : ModelBase
    {
        internal override bool IsValid()
        {
            return false;
        }
    }
}