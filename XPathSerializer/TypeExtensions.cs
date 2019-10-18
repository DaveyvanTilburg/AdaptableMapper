using System;

namespace XPathSerialization
{
    public static class TypeExtensions
    {
        public static Adaptable CreateAdaptable(this Type type)
        {
            Type listItemType = type.GetGenericArguments()[0];
            return Activator.CreateInstance(listItemType) as Adaptable;
        }
    }
}