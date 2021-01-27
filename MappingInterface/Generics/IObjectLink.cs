using System;

namespace MappingFramework.MappingInterface.Generics
{
    public interface IObjectLink
    {
        ObjectComponentDisplayType Type();
        string Name();
        Type PropertyType();
        void Update(object value);
        object Value();
        Action<object> UpdateAction();
    }
}