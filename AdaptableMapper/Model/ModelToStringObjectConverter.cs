using AdaptableMapper.Memory.Language;
using Newtonsoft.Json;

namespace AdaptableMapper.Model
{
    public sealed class ModelToStringObjectConverter : ObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is ModelBase adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return string.Empty;
            }

            string result = JsonConvert.SerializeObject(adaptable);
            return result;
        }
    }
}