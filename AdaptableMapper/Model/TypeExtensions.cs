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
                Process.ProcessObservable.GetInstance().Raise($"MODEL#23; Model has a list property with generic type {listItemType.Name} that is not a Model", "error");
                return new NullModel();
            }

            return result;
        }
    }
}