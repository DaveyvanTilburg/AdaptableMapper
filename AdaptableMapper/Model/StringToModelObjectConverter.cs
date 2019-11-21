using System;
using AdaptableMapper.Contexts;
using AdaptableMapper.Model.Language;

namespace AdaptableMapper.Model
{
    public sealed class StringToModelObjectConverter : ObjectConverter
    {
        public StringToModelObjectConverter(ModelTargetInstantiatorSource modelTargetInstantiatorSource)
        {
            ModelTargetInstantiatorSource = modelTargetInstantiatorSource;
        }

        public ModelTargetInstantiatorSource ModelTargetInstantiatorSource { get; set; }

        public object Convert(object source)
        {
            if (!(source is string input))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#27; source is not of expected type string", "error", source);
                return new NullModel();
            }

            Type sourceType;
            try
            {
                sourceType = Activator.CreateInstance(
                    ModelTargetInstantiatorSource.AssemblyFullName,
                    ModelTargetInstantiatorSource.TypeFullName
                ).Unwrap().GetType();
            }
            catch (Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#28; could not instantiate sourceType", "error", ModelTargetInstantiatorSource, exception.GetType().Name, exception.Message);
                return new NullModel();
            }

            object result;
            try
            {
                result = JsonSerializer.Deserialize(sourceType, input);
            }
            catch(Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#29; could not deserialize to type", "error", source, exception.GetType().Name, exception.Message);
                return new NullModel();
            }

            if (!(result is ModelBase))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#30; sourceType is not of type modelBase", "error", ModelTargetInstantiatorSource);
                return new NullModel();
            }

            return result;
        }
    }
}