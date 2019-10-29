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
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return string.Empty;
            }

            string result = JsonConvert.SerializeObject(model);
            return result;
        }
    }
}