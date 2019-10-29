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
                Errors.ErrorObservable.GetInstance().Raise($"Model has property with type {type.Name} that is not an Model, that is being traversed");
                return new NullModel();
            }

            return result;
        }
    }
}