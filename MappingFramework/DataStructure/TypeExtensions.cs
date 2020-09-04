using System;
using MappingFramework.Process;

namespace MappingFramework.DataStructure
{
    public static class TypeExtensions
    {
        public static TraversableDataStructure CreateDataStructure(this Type type)
        {
            Type listItemType = type.GetGenericArguments()[0];
            object instance = Activator.CreateInstance(listItemType);

            if (!(instance is TraversableDataStructure result))
            {
                ProcessObservable.GetInstance().Raise($"MODEL#23; Model has a list property with generic type {listItemType.Name} that is not a Model", "error");
                return new NullDataStructure();
            }

            return result;
        }
    }
}