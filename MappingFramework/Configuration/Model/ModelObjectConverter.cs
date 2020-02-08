using MappingFramework.Converters;
using MappingFramework.Model;

namespace MappingFramework.Configuration.Model
{
    public sealed class ModelObjectConverter : ObjectConverter, ResolvableByTypeId
    {
        public const string _typeId = "0ff249bd-3dc5-4125-b130-42fe89cb31eb";
        public string TypeId => _typeId;

        public ModelObjectConverter() { }

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