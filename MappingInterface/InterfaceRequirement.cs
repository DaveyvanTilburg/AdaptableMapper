using System;
using System.Reflection;

namespace MappingFramework.MappingInterface
{
    public class InterfaceRequirement
    {
        private readonly PropertyInfo _propertyInfo;
        
        public InterfaceRequirement(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public InterfaceRequirementType Type() =>
            _propertyInfo.PropertyType == typeof(string) ? InterfaceRequirementType.TextBox :
            _propertyInfo.PropertyType == typeof(bool) ? InterfaceRequirementType.CheckBox :
            _propertyInfo.PropertyType.IsEnum ? InterfaceRequirementType.RadioGroupBox :
                InterfaceRequirementType.Undefined;

        public string Name() => _propertyInfo.Name;

        public Type PropertyType() => _propertyInfo.PropertyType;
    }
}