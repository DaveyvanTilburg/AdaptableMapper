using System;
using System.Collections;
using System.Reflection;
using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.Traversals;
using MappingFramework.ValueMutations;
using MappingFramework.ValueMutations.Traversals;

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
            _propertyInfo.PropertyType == typeof(int) ? InterfaceRequirementType.NumberBox :
            _propertyInfo.PropertyType == typeof(char) ? InterfaceRequirementType.CharBox :
            _propertyInfo.PropertyType.IsEnum ? InterfaceRequirementType.RadioGroupBox :
            
            _propertyInfo.PropertyType == typeof(ObjectConverter) ? InterfaceRequirementType.Item :
            _propertyInfo.PropertyType == typeof(TargetInstantiator) ? InterfaceRequirementType.Item :
            _propertyInfo.PropertyType == typeof(ResultObjectConverter) ? InterfaceRequirementType.Item :

            _propertyInfo.PropertyType == typeof(GetValueTraversal) ? InterfaceRequirementType.Item :
            _propertyInfo.PropertyType == typeof(SetValueTraversal) ? InterfaceRequirementType.Item :
            _propertyInfo.PropertyType == typeof(ValueMutation) ? InterfaceRequirementType.Item :
            _propertyInfo.PropertyType == typeof(Condition) ? InterfaceRequirementType.Item :
            _propertyInfo.PropertyType == typeof(GetValueStringTraversal) ? InterfaceRequirementType.Item :
            
            _propertyInfo.PropertyType == typeof(Mapping) ? InterfaceRequirementType.Direct :
            _propertyInfo.PropertyType == typeof(ContextFactory) ? InterfaceRequirementType.Direct :

            typeof(IEnumerable).IsAssignableFrom(_propertyInfo.PropertyType) ? InterfaceRequirementType.List :
                InterfaceRequirementType.Undefined;

        public string Name() => _propertyInfo.Name;

        public Type PropertyType() => _propertyInfo.PropertyType;

        public void Update(object value) => _propertyInfo.SetValue(_subject, value);

        public Action<object> UpdateAction() => Update;
    }
}