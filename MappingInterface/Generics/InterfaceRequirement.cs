using System;
using System.Reflection;
using MappingFramework.Traversals;
using MappingFramework.ValueMutations;

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
            _propertyInfo.PropertyType == typeof(GetValueTraversal) ? InterfaceRequirementType.GetValueTraversal :
            _propertyInfo.PropertyType == typeof(SetValueTraversal) ? InterfaceRequirementType.SetValueTraversal :
            _propertyInfo.PropertyType == typeof(ValueMutation) ? InterfaceRequirementType.ValueMutation :
                InterfaceRequirementType.Undefined;

        public string Name() => _propertyInfo.Name;

        public Type PropertyType() => _propertyInfo.PropertyType;

        public void Update(object value) => _propertyInfo.SetValue(_subject, value);

        public PropertyInfo PropertyInfo() => _propertyInfo;
    }
}