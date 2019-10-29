using AdaptableMapper.Contexts;
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
            return Activator.CreateInstance(AssemblyName, TypeName).Unwrap();
        }
    }
}