using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Traversals;
using MappingFramework.ValueMutations;

namespace MappingFramework.Visitors
{
    internal class CompositionValidationVisitor : IVisitor
    {
        private readonly List<Information> _information;
        
        private List<ContentType> _sourceTypes;
        private List<ContentType> _targetTypes;
        
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



        public void Visit(SourceCreator sourceCreator)
            => _sourceTypes = ClassContentType(sourceCreator);

        public void Visit(TargetCreator targetCreator)
            => _targetTypes = ClassContentType(targetCreator);



        public void Visit(ResultObjectCreator resultObjectCreator)
            => Validate(_targetTypes, resultObjectCreator);

        public void Visit(GetValueTraversal getValueTraversal)
            => Validate(_sourceTypes, getValueTraversal);

        public void Visit(GetListValueTraversal getListValueTraversal)
            => Validate(_sourceTypes, getListValueTraversal);

        public void Visit(SetValueTraversal setValueTraversal)
            => Validate(_targetTypes, setValueTraversal);

        public void Visit(Condition condition)
            => Validate(_sourceTypes, condition);

        public void Visit(ValueMutation valueMutation)
            => Validate(_sourceTypes, valueMutation);

        public void Visit(GetTemplateTraversal getTemplateTraversal)
            => Validate(_targetTypes, getTemplateTraversal);

        public void Visit(ChildCreator childCreator)
            => Validate(_targetTypes, childCreator);

        private void Validate<T>(List<ContentType> contentType, T subject)
        {
            if (subject == null)
            {
                _information.Add(new Information($"Composition incomplete, required object is empty, type: {typeof(T).Name}", InformationType.Error));
                return;
            }

            NullChecks(subject);
            
            if (subject is IVisitable visitable)
                visitable.Receive(this);
            else
                CompareTypes(contentType, subject);
        }
        
        private void CompareTypes(List<ContentType> contentType, object subject)
        {
            List<ContentType> subjectContentType = ClassContentType(subject);

            if (subjectContentType.Contains(ContentType.Any) || subjectContentType.Intersect(contentType).Any())
            {}
            else
                _information.Add(new Information($"Inconsistent types in the composition, expected: {string.Join(",", contentType)}, found: {string.Join(",", subjectContentType)}, objectType: {subject.GetType().Name}", InformationType.Error));
        }
        
        private void NullChecks(object subject)
        {
            var propertyInfos = subject.GetType().GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
                if (propertyInfo.PropertyType.IsClass || propertyInfo.PropertyType.IsInterface)
                    if (propertyInfo.GetValue(subject) == null)
                        _information.Add(new Information($"Composition incomplete, object missing of type {propertyInfo.PropertyType.Name}", InformationType.Error));
        }
        
        private List<ContentType> ClassContentType(object subject)
        {
            Type type = subject.GetType();

            ContentTypeAttribute attribute = (ContentTypeAttribute)type.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(ContentTypeAttribute));
            return attribute?.ContentType ?? new List<ContentType>{ ContentType.Undefined };
        }
    }
}