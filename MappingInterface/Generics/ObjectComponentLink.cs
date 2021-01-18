﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.Traversals;
using MappingFramework.ValueMutations;
using MappingFramework.ValueMutations.Traversals;

namespace MappingFramework.MappingInterface.Generics
{
    public class ObjectComponentLink
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly object _subject;

        public ObjectComponentLink(PropertyInfo propertyInfo, object subject)
        {
            _propertyInfo = propertyInfo;
            _subject = subject;
        }

        public ObjectComponentDisplayType Type() =>
            _propertyInfo.PropertyType == typeof(string) ? ObjectComponentDisplayType.TextBox :
            _propertyInfo.PropertyType == typeof(bool) ? ObjectComponentDisplayType.CheckBox :
            _propertyInfo.PropertyType == typeof(int) ? ObjectComponentDisplayType.NumberBox :
            _propertyInfo.PropertyType == typeof(char) ? ObjectComponentDisplayType.CharBox :
            _propertyInfo.PropertyType.IsEnum ? ObjectComponentDisplayType.RadioGroupBox :
            
            _propertyInfo.PropertyType == typeof(ObjectConverter) ? ObjectComponentDisplayType.Item :
            _propertyInfo.PropertyType == typeof(TargetInstantiator) ? ObjectComponentDisplayType.Item :
            _propertyInfo.PropertyType == typeof(ResultObjectConverter) ? ObjectComponentDisplayType.Item :

            _propertyInfo.PropertyType == typeof(GetValueTraversal) ? ObjectComponentDisplayType.Item :
            _propertyInfo.PropertyType == typeof(SetValueTraversal) ? ObjectComponentDisplayType.Item :
            _propertyInfo.PropertyType == typeof(ValueMutation) ? ObjectComponentDisplayType.Item :
            _propertyInfo.PropertyType == typeof(Condition) ? ObjectComponentDisplayType.Item :
            _propertyInfo.PropertyType == typeof(GetValueStringTraversal) ? ObjectComponentDisplayType.Item :
            _propertyInfo.PropertyType == typeof(GetListValueTraversal) ? ObjectComponentDisplayType.Item :
            _propertyInfo.PropertyType == typeof(GetTemplateTraversal) ? ObjectComponentDisplayType.Item :
            _propertyInfo.PropertyType == typeof(ChildCreator) ? ObjectComponentDisplayType.Item :

            _propertyInfo.PropertyType == typeof(Mapping) ? ObjectComponentDisplayType.Direct :
            _propertyInfo.PropertyType == typeof(ContextFactory) ? ObjectComponentDisplayType.Direct :
            _propertyInfo.PropertyType == typeof(MappingScopeComposite) ? ObjectComponentDisplayType.Direct :
            _propertyInfo.PropertyType == typeof(AdditionalSourceEntry) ? ObjectComponentDisplayType.Direct :

            _propertyInfo.PropertyType == typeof(List<AdditionalSource>) ? ObjectComponentDisplayType.List :
            typeof(IEnumerable).IsAssignableFrom(_propertyInfo.PropertyType) ? ObjectComponentDisplayType.List :
            ObjectComponentDisplayType.Undefined;

        public string Name() => _propertyInfo.Name;

        public Type PropertyType() => _propertyInfo.PropertyType;

        public void Update(object value) => _propertyInfo.SetValue(_subject, value);

        public Action<object> UpdateAction() => Update;
    }
}