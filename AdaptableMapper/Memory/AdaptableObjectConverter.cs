using AdaptableMapper.Contexts;
using AdaptableMapper.Memory.Language;

namespace AdaptableMapper.Memory
{
    public class AdaptableObjectConverter : ObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is Adaptable adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Adaptable");
                return string.Empty;
            }

            return adaptable;
        }
    }
}