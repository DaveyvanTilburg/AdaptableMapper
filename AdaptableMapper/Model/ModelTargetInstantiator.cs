using AdaptableMapper.Contexts;
using AdaptableMapper.Model.Language;
using System;

namespace AdaptableMapper.Model
{
    public sealed class ModelTargetInstantiator : TargetInstantiator
    {
        public object Create(object source)
        {
            if (!(source is string template))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#25; Source is not of expected type string", "error", source, source?.GetType().Name);
                return new NullModel();
            }

            ModelTargetInstantiatorSource modelTargetInstantiatorSource;
            try
            {
                modelTargetInstantiatorSource = JsonSerializer.Deserialize<ModelTargetInstantiatorSource>(template);
            }
            catch(Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise($"MODEL#26; string does not contain a serialized ModelTargetInstantiatorSource", "error", template, exception.GetType().Name, exception.Message);
                return new NullModel();
            }

            object result;
            try
            {
                result = Activator.CreateInstance(modelTargetInstantiatorSource.AssemblyFullName, modelTargetInstantiatorSource.TypeFullName).Unwrap();
            }
            catch(Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise($"MODEL#24; assembly and typename could not be instantiated", "error", modelTargetInstantiatorSource, exception.GetType().Name, exception.Message);
                return new NullModel();
            }

            if (!(result is ModelBase))
            {
                Process.ProcessObservable.GetInstance().Raise($"MODEL#31; instantiated object is not of type ModelBase", "error", modelTargetInstantiatorSource);
                return new NullModel();
            }

            return result;
        }
    }
}