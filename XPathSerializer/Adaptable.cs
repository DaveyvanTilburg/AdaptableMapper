using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace XPathSerialization
{
    public abstract class Adaptable
    {
        public void SetPropertyValue(string objectPath, string value)
        {
            var path = new Stack<string>(GetPath(objectPath));
            string propertyName = path.Pop();

            Adaptable adaptable = this;
            if (path.Count > 0)
                adaptable = GetChild(new Queue<string>(path));

            adaptable.SetValue(propertyName, value);
        }

        public IList GetProperty(string objectPath)
        {
            var path = new Stack<string>(GetPath(objectPath));
            string propertyName = path.Pop();

            Adaptable adaptable = this;
            if (path.Count > 0)
                adaptable = GetChild(new Queue<string>(path));

            PropertyInfo property = adaptable.GetType().GetProperty(propertyName);
            return property.GetValue(adaptable) as IList;
        }

        private Adaptable GetChild(Queue<string> path)
        {
            string step = path.Dequeue();

            PropertyInfo property = GetType().GetProperty(step);
            var propertyValue = property.GetValue(this) as IList;

            Adaptable entry = property.PropertyType.CreateAdaptable();
            propertyValue.Add(entry);

            if (path.Count > 0)
                return entry.GetChild(path);

            return entry;
        }

        private void SetValue(string propertyName, string value)
        {
            PropertyInfo property = GetType().GetProperty(propertyName);
            property.SetValue(this, value);
        }

        private static IEnumerable<string> GetPath(string objectPath)
        {
            return objectPath.Split('/');
        }
    }
}