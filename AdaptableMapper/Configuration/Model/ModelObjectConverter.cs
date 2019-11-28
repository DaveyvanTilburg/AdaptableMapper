using AdaptableMapper.Model;

namespace AdaptableMapper.Configuration.Model
{
    public sealed class ModelObjectConverter : ObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is ModelBase model))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#17; source is not of expected type Model", "error", source);
                return new NullModel();
            }

            return model;
        }
    }
}