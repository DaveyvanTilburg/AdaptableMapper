using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdaptableMapper.Builder.Interpreters
{
    internal class DirectAddTo : Interpreter
    {
        public DirectAddTo()
        {
            _skip = new List<string>
            {
                "MappingConfiguration"
            };
        }

        private readonly List<string> _skip;

        public string CommandName => "direct-add-to";
        public void Receive(Visitor visitor)
        {
            var path = visitor.Command.Next();
            var pathParts = new Stack<string>(path.Split('.'));

            var lastInList = pathParts.Pop();

            object property = visitor.Result;
            foreach (string pathPart in pathParts.Except(_skip, StringComparer.OrdinalIgnoreCase).ToList())
                property = NavigateToProperty(property, pathPart);

            AddToList(property, lastInList, visitor.Subject);
            visitor.Subject = null;
        }

        private object NavigateToProperty(object source, string propertyName)
        {
            Type sourceType = source.GetType();
            PropertyInfo propertyInfo = sourceType.GetProperty(propertyName);

            return propertyInfo?.GetValue(source);
        }

        private void AddToList(object source, string propertyName, object value)
        {
            Type sourceType = source.GetType();
            PropertyInfo propertyInfo = sourceType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            IList target = propertyInfo?.GetValue(source) as IList;

            target.Add(value);
        }
    }
}