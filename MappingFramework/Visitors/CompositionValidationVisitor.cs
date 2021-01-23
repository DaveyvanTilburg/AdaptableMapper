using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Process;
using MappingFramework.Traversals;
using MappingFramework.ValueMutations;

namespace MappingFramework.Visitors
{
    internal class CompositionValidationVisitor : IVisitor
    {
        private readonly List<Information> _information;
        
        private ContentType _sourceType;
        private ContentType _targetType;
        
        public CompositionValidationVisitor()
        {
            _information = new List<Information>();
        }

        public List<Information> Feedback()
            => _information;

        public void Visit(MappingConfiguration mappingConfiguration)
            => ((IVisitable)mappingConfiguration).Receive(this);

        public void Visit(ContextFactory contextFactory)
            => ((IVisitable)contextFactory).Receive(this);

        public void Visit(Mapping mapping)
            => ((IVisitable)mapping).Receive(this);

        public void Visit(MappingScopeComposite mappingScopeComposite)
            => ((IVisitable)mappingScopeComposite).Receive(this);



        public void Visit(ObjectConverter objectConverter)
            => _sourceType = ClassContentType(objectConverter);

        public void Visit(TargetInstantiator targetInstantiator)
            => _targetType = ClassContentType(targetInstantiator);



        public void Visit(ResultObjectConverter resultObjectConverter)
            => Validate(_targetType, resultObjectConverter);

        public void Visit(GetValueTraversal getValueTraversal)
            => Validate(_sourceType, getValueTraversal);

        public void Visit(GetListValueTraversal getListValueTraversal)
            => Validate(_sourceType, getListValueTraversal);

        public void Visit(SetValueTraversal setValueTraversal)
            => Validate(_targetType, setValueTraversal);

        public void Visit(Condition condition)
            => Validate(_sourceType, condition);

        public void Visit(ValueMutation valueMutation)
            => Validate(_sourceType, valueMutation);

        public void Visit(GetTemplateTraversal getTemplateTraversal)
            => Validate(_targetType, getTemplateTraversal);

        public void Visit(ChildCreator childCreator)
            => Validate(_targetType, childCreator);

        private void Validate(ContentType contentType, object subject)
        {
            if (subject == null)
            {
                _information.Add(new Information($"Composition incomplete, object empty : {subject.GetType().Name}", InformationType.Error));
                return;
            }

            NullChecks(subject);
            
            if (subject is IVisitable visitable)
                visitable.Receive(this);
            else
                CompareTypes(contentType, subject);
        }
        
        private void CompareTypes(ContentType contentType, object subject)
        {
            ContentType subjectContentType = ClassContentType(subject);

            if (contentType != subjectContentType)
                _information.Add(new Information($"Inconsistent types in the composition, expected: {contentType}, found: {subjectContentType}, objectType: {subject.GetType().Name}", InformationType.Error));
        }
        
        private void NullChecks(object subject)
        {
            var propertyInfos = subject.GetType().GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
                if (propertyInfo.PropertyType.IsClass || propertyInfo.PropertyType.IsInterface)
                    if (propertyInfo.GetValue(subject) == null)
                        _information.Add(new Information($"Composition incomplete, object missing of type {propertyInfo.PropertyType.Name}", InformationType.Error));
        }
        
        private ContentType ClassContentType(object subject)
        {
            Type type = subject.GetType();

            ContentTypeAttribute attribute = (ContentTypeAttribute)type.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(ContentTypeAttribute));
            return attribute?.ContentType ?? ContentType.Undefined;
        }
    }
}