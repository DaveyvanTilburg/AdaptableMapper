namespace AdaptableMapper.Model
{
    public class NullModel : ModelBase
    {
        internal override bool IsValid()
        {
            return false;
        }
    }
}