using System;
using MappingFramework.Configuration;

namespace MappingFramework.Languages.DataStructure
{
    public static class TypeExtensions
    {
        public static TraversableDataStructure CreateDataStructure(this Type type, Context context)
        {
            Type listItemType = type.GetGenericArguments()[0];
            object instance = Activator.CreateInstance(listItemType);

            if (!(instance is TraversableDataStructure result))
            {
                context.InvalidType(instance, typeof(TraversableDataStructure));
                return new NullDataStructure();
            }

            return result;
        }
    }
}