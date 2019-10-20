using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XPathSerialization
{
    public abstract class Adaptable
    {
        private Adaptable _parent;
        public void SetParent(Adaptable parent)
            => _parent = parent;

        public Adaptable GetOrCreateAdaptable(Queue<string> path)
        {
            Adaptable adaptable = this;
            if (path.Count > 0)
                adaptable = NavigateAndCreatePath(new Queue<string>(path));

            return adaptable;
        }

        public IList GetProperty(string propertyName)
        {
            PropertyInfo property = this.GetType().GetProperty(propertyName);
            return property.GetValue(this) as IList;
        }

        private Adaptable NavigateAndCreatePath(Queue<string> path)
        {
            string step = path.Dequeue();

            PropertyInfo property = GetType().GetProperty(step);
            var propertyValue = property.GetValue(this) as IList;

            Adaptable next = property.PropertyType.CreateAdaptable();
            propertyValue.Add(next);

            if (path.Count > 0)
                return next.NavigateAndCreatePath(path);

            return next;
        }

        public Adaptable NavigateToAdaptable(Queue<string> path)
        {
            if (path.Count == 0)
                return this;

            string step = path.Dequeue();

            Adaptable next;
            if (step.Equals(".."))
                next = _parent;
            else if(step.TryGetObjectFilter(out AdaptableFilter filter))
            {
                PropertyInfo property = GetType().GetProperty(filter.PropertyName);
                var propertyValue = property.GetValue(this) as IEnumerable<Adaptable>;

                next = propertyValue.FirstOrDefault(a => a.GetValue(filter.Name).Equals(filter.Value));
            }
            else
            {
                PropertyInfo property = GetType().GetProperty(step);
                var propertyValue = property.GetValue(this) as IEnumerable<Adaptable>;

                next = propertyValue.First() as Adaptable;
            }

            if (path.Count > 0)
                return next.NavigateToAdaptable(path);

            return next;
        }

        public void SetValue(string propertyName, string value)
        {
            PropertyInfo property = GetType().GetProperty(propertyName);
            property.SetValue(this, value);
        }

        public string GetValue(string propertyName)
        {
            PropertyInfo property = GetType().GetProperty(propertyName);
            return property.GetValue(this).ToString();
        }
    }
}