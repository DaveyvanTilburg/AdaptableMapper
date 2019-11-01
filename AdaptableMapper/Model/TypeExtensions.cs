using AdaptableMapper.Model.Language;
using System;

namespace AdaptableMapper.Model
{
    public static class TypeExtensions
    {
        public static ModelBase CreateModel(this Type type)
        {
            Type listItemType = type.GetGenericArguments()[0];
            object instance = Activator.CreateInstance(listItemType);

            if (!(instance is ModelBase result))
            {
                Errors.ProcessObservable.GetInstance().Raise($"MODEL#23; Model has property with type {type.Name} that is not a Model", "error");
                return new NullModel();
            }

            return result;
        }
    }
}