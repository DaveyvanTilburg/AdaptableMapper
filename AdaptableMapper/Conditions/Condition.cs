using AdaptableMapper.Configuration;

namespace AdaptableMapper.Conditions
{
    public interface Condition
    {
        bool Validate(Context context);
    }
}