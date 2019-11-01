using AdaptableMapper.Model.Language;
using Newtonsoft.Json;

namespace AdaptableMapper.Model
{
    public sealed class ModelToStringObjectConverter : ObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is ModelBase model))
            {
                Errors.ProcessObservable.GetInstance().Raise("MODEL#20; source is not of expected type Model", "error", source);
                return string.Empty;
            }

            string result = JsonConvert.SerializeObject(model);
            return result;
        }
    }
}