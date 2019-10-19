using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace XPathSerialization
{
    public abstract class Adaptable
    {
        private Adaptable _parent;
        public void SetParent(Adaptable parent)
            => _parent = parent;

        public void SetPropertyValue(string objectPath, string value)
        {
            var path = new Stack<string>(GetPath(objectPath));
            string propertyName = path.Pop();

            Adaptable adaptable = this;
            if (path.Count > 0)
                adaptable = NavigateToPath(new Queue<string>(path));

            adaptable.SetValue(propertyName, value);
        }

        public string GetPropertyValue(string objectPath)
        {
            var path = new Stack<string>(GetPath(objectPath));
            string propertyName = path.Pop();

            Adaptable adaptable = this;
            if (path.Count > 0)
                adaptable = NavigateToPath(new Queue<string>(path));

            return adaptable.GetValue(propertyName);
        }

        public (IList property, Adaptable parent) GetProperty(string objectPath)
        {
            var path = new Stack<string>(GetPath(objectPath));
            string propertyName = path.Pop();

            Adaptable adaptable = this;
            if (path.Count > 0)
                adaptable = NavigateToPath(new Queue<string>(path));

            PropertyInfo property = adaptable.GetType().GetProperty(propertyName);
            return (property.GetValue(adaptable) as IList, adaptable);
        }

        private Adaptable NavigateToPath(Queue<string> path)
        {
            string step = path.Dequeue();

            Adaptable next;
            if (step.Equals(".."))
                next = _parent;
            else
            {
                PropertyInfo property = GetType().GetProperty(step);
                var propertyValue = property.GetValue(this) as IList;

                next = property.PropertyType.CreateAdaptable();
                propertyValue.Add(next);
            }

            if (path.Count > 0)
                return next.NavigateToPath(path);

            return next;
        }

        private void SetValue(string propertyName, string value)
        {
            PropertyInfo property = GetType().GetProperty(propertyName);
            property.SetValue(this, value);
        }

        private string GetValue(string propertyName)
        {
            PropertyInfo property = GetType().GetProperty(propertyName);
            return property.GetValue(this).ToString();
        }

        private static IEnumerable<string> GetPath(string objectPath)
        {
            return objectPath.Split('/');
        }
    }
}