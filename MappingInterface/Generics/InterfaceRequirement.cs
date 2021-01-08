using System;
using System.Reflection;

namespace MappingFramework.MappingInterface.Generics
{
    public class InterfaceRequirement
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly object _subject;

        public InterfaceRequirement(PropertyInfo propertyInfo, object subject)
        {
            _propertyInfo = propertyInfo;
            _subject = subject;
        }

        public InterfaceRequirementType Type() =>
            _propertyInfo.PropertyType == typeof(string) ? InterfaceRequirementType.TextBox :
            _propertyInfo.PropertyType == typeof(bool) ? InterfaceRequirementType.CheckBox :
            _propertyInfo.PropertyType.IsEnum ? InterfaceRequirementType.RadioGroupBox :
                InterfaceRequirementType.Undefined;

        public string Name() => _propertyInfo.Name;

        public Type PropertyType() => _propertyInfo.PropertyType;

        public void Update(object value) => _propertyInfo.SetValue(_subject, value);
    }
}