using AdaptableMapper.Model.Language;
using Newtonsoft.Json;

namespace AdaptableMapper.Model
{
    public sealed class ModelToStringObjectConverter : ResultObjectConverter
    {
        public object Convert(object source)
        {
            if (!(source is ModelBase model))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#20; source is not of expected type Model", "error", source);
                return new NullModel();
            }

            string result = JsonConvert.SerializeObject(model);
            return result;
        }
    }
}