using AdaptableMapper.Contexts;
using AdaptableMapper.Memory.Language;

namespace AdaptableMapper.Memory
{
    public sealed class ModelObjectConverter : ObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is ModelBase adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return string.Empty;
            }

            return adaptable;
        }
    }
}