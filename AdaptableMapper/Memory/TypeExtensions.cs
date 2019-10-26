using AdaptableMapper.Memory.Language;
using System;

namespace AdaptableMapper.Memory
{
    public static class TypeExtensions
    {
        public static Adaptable CreateAdaptable(this Type type)
        {
            Type listItemType = type.GetGenericArguments()[0];
            object instance = Activator.CreateInstance(listItemType);

            if (!(instance is Adaptable result))
                throw new InvalidAdaptablePathException($"Adaptable has property with type {type.Name} that is not an adaptable, that is being traversed");

            return result;
        }
    }
}