using System;
using System.Collections.Generic;
using MappingFramework.Process;

namespace MappingFramework.Configuration
{
    public sealed class Context
    {
        private readonly List<Information> _information;

        public object Source { get; set; }
        public object Target { get; set; }
        public AdditionalSourceValues AdditionalSourceValues { get; set; }
        
        public Context()
        {
            _information = new List<Information>();
        }

        public Context(
            object source,
            object target,
            AdditionalSourceValues additionalSourceValues) : this()
        {
            Source = source;
            Target = target;
            AdditionalSourceValues = additionalSourceValues;
            
        }

        public void AddInformation(string message, InformationType type)
            => _information.Add(new Information(message, type));

        public void AddInformation(string message, InformationType type, Exception exception)
            => _information.Add(new Information(message, type, exception));

        public void PropertyIsEmpty(object subject, string propertyName)
            => _information.Add(new Information($"Property {propertyName} empty of object {subject.GetType().Name}", InformationType.Warning));

        public void ResultIsEmpty(object subject)
            => _information.Add(new Information($"{subject.GetType().Name} resulted in a empty value", InformationType.Warning));
        
        public void NavigationResultIsEmpty(string path)
            => _information.Add(new Information($"Path {path} resulted in a empty value", InformationType.Warning));

        public void TemplatePathNeedsAParent(string path)
            => _information.Add(new Information($"Path resulted in an item that has no parent, path: {path}", InformationType.Warning));

        public void InvalidType(object subject, Type expectedType)
            => _information.Add(new Information($"Object {subject.GetType().Name} is not of expected type {expectedType.Name}", InformationType.Error));

        public void InvalidInput(object subject, Type expectedType)
            => _information.Add(new Information($"Input of type {subject.GetType().Name} provided as a parameter to 'map' is not of expected type {expectedType.Name}", InformationType.Error));

        public void OperationFailed(object operation, Exception exception)
            => _information.Add(new Information($"Operation: {operation.GetType().Name} failed", InformationType.Error, exception));

        public void NavigationFailed(string path)
            => _information.Add(new Information($"Path could not be resolved: {path}", InformationType.Warning));
        
        public void NavigationException(string path, Exception exception)
            => _information.Add(new Information($"Navigation resulted in an exception, path: {path}", InformationType.Warning, exception));

        public void NavigationInvalid(string path, string message)
            => _information.Add(new Information($"Path is invalid, path: {path}, message: {message}", InformationType.Error));

        public void NavigationInvalid(string path, string message, object subject)
            => _information.Add(new Information($"Path is invalid for {subject.GetType().Name}, path: {path}, message: {message}", InformationType.Error));

        public string AdditionalValue(string name, string key)
            => AdditionalSourceValues.GetValue(name, key, this);

        public List<Information> Information() => _information;
    }
}