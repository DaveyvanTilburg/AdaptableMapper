using AdaptableMapper.Contexts;
using AdaptableMapper.Model.Language;

namespace AdaptableMapper.Model
{
    public sealed class ModelObjectConverter : ObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is ModelBase model))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return string.Empty;
            }

            return model;
        }
    }
}