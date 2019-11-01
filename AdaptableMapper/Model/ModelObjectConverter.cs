using AdaptableMapper.Model.Language;

namespace AdaptableMapper.Model
{
    public sealed class ModelObjectConverter : ObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is ModelBase model))
            {
                Errors.ProcessObservable.GetInstance().Raise("MODEL#17; source is not of expected type Model", "error", source);
                return string.Empty;
            }

            return model;
        }
    }
}