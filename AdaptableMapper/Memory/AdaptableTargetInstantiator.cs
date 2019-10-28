using AdaptableMapper.Contexts;
using System;

namespace AdaptableMapper.Memory
{
    public class AdaptableTargetInstantiator : TargetInstantiator
    {
        public AdaptableTargetInstantiator(string assemblyName, string typeName)
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