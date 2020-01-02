using AdaptableMapper.Model;
using Newtonsoft.Json;

namespace AdaptableMapper.Configuration.Model
{
    public sealed class ModelToStringObjectConverter : ResultObjectConverter, SerializableByTypeId
    {
        public const string _typeId = "5e251dd5-ba6e-4de4-8973-8ed67d0e1991";
        public string TypeId => _typeId;

        public ModelToStringObjectConverter() { }

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