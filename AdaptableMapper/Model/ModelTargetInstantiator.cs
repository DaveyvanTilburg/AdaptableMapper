using AdaptableMapper.Contexts;
using AdaptableMapper.Model.Language;
using System;

namespace AdaptableMapper.Model
{
    public sealed class ModelTargetInstantiator : TargetInstantiator
    {
        public ModelTargetInstantiator(string assemblyName, string typeName)
        {
            AssemblyName = assemblyName;
            TypeName = typeName;
        }

        public string AssemblyName { get; set; }
        public string TypeName { get; set; }

        public object Create()
        {
            object result;
            try
            {
                result = Activator.CreateInstance(AssemblyName, TypeName).Unwrap();
            }
            catch(Exception exception)
            {
                Process.ProcessObservable.GetInstance().Raise($"MODEL#24; assembly and typename could not be instantiated", "error", AssemblyName, TypeName, exception.GetType().Name, exception.Message);
                return new NullModel();
            }

            return result;
        }
    }
}