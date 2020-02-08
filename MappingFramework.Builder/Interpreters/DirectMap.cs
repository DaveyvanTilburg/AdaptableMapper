using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MappingFramework.Builder.Interpreters
{
    internal class DirectMap : Interpreter
    {
        public DirectMap()
        {
            _skip = new List<string>
            {
                "MappingConfiguration"
            };
        }

        private readonly List<string> _skip;
        public string CommandName => "direct-map";
        public void Receive(Visitor visitor)
        {
            var path = visitor.Command.Next();
            var pathParts = new Stack<string>(path.Split('.'));

            var lastInList = pathParts.Pop();

            object property = visitor.Result;
            foreach (string pathPart in pathParts.Except(_skip, StringComparer.OrdinalIgnoreCase).ToList())
                property = NavigateToProperty(property, pathPart);

            SetPropertyValue(property, lastInList, visitor.Subject);
            visitor.Subject = null;
        }

        private object NavigateToProperty(object source, string propertyName)
        {
            Type sourceType = source.GetType();
            PropertyInfo propertyInfo = sourceType.GetProperty(propertyName);

            return propertyInfo?.GetValue(source);
        }

        private void SetPropertyValue(object source, string propertyName, object value)
        {
            Type sourceType = source.GetType();
            PropertyInfo propertyInfo = sourceType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            propertyInfo?.SetValue(source, value);
        }
    }
}