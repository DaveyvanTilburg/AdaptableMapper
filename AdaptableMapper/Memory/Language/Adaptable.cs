using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdaptableMapper.Memory.Language
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

        private Adaptable NavigateAndCreatePath(Queue<string> path)
        {
            string step = path.Dequeue();

            PropertyInfo property = GetPropertyInfo(step);
            IList propertyValue = GetIListFromProperty(property, step);

            Adaptable next = property.PropertyType.CreateAdaptable();
            propertyValue.Add(next);

            if (path.Count > 0)
                return next.NavigateAndCreatePath(path);

            return next;
        }

        public IList GetListProperty(string propertyName)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(propertyName);
            return GetIListFromProperty(propertyInfo, propertyName);
        }

        private IList GetIListFromProperty(PropertyInfo propertyInfo, string propertyName)
        {
            object property = propertyInfo.GetValue(this);

            if (property == null)
                Errors.ErrorObservable.GetInstance().Raise($"Property {propertyName} does not exist on {this.GetType().Name}");

            if ((property is IList listProperty))
                return listProperty;
            else
            {
                Errors.ErrorObservable.GetInstance().Raise($"Property {propertyName} is not traversable, it is not a list of adaptable");
                return new List<Adaptable>();
            }
        }

        public Adaptable NavigateToAdaptable(Queue<string> path)
        {
            if (path.Count == 0)
                return this;

            string step = path.Dequeue();

            Adaptable next;
            if (step.Equals(".."))
            {
                next = _parent;
                if (next == null)
                    Errors.ErrorObservable.GetInstance().Raise($"Parent node was null while navigating to .. of type {this.GetType().Name}");
            }
            else if(step.TryGetObjectFilter(out AdaptableFilter filter))
            {
                IEnumerable<Adaptable> propertyValue = GetEnumerableProperty(filter.AdaptableName);
                next = propertyValue.FirstOrDefault(a => a.GetValue(filter.PropertyName).Equals(filter.Value));

                if (next == null)
                    Errors.ErrorObservable.GetInstance().Raise($"No match found for filter on list with name {filter.AdaptableName} with a value that has a {filter.PropertyName} with value {filter.Value}");
            }
            else
            {
                IEnumerable<Adaptable> propertyValue = GetEnumerableProperty(step);
                next = propertyValue.FirstOrDefault();

                if (next == null)
                    Errors.ErrorObservable.GetInstance().Raise($"No items found in {step} in type {this.GetType().Name}");
            }

            if (next == null)
                return this;

            if (path.Count > 0)
                return next.NavigateToAdaptable(path);

            return next;
        }

        private IEnumerable<Adaptable> GetEnumerableProperty(string propertyName)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(propertyName);
            object property = propertyInfo.GetValue(this);

            if(property == null)
                Errors.ErrorObservable.GetInstance().Raise($"Property {propertyName} does not exist on {this.GetType().Name}");

            if ((property is IEnumerable<Adaptable> enumerableProperty))
            {
                return enumerableProperty;
            }
            else
            {
                Errors.ErrorObservable.GetInstance().Raise($"Property {propertyName} is not traversable, it is not a list of adaptable");
                return new List<Adaptable>();
            }
        }

        public void SetValue(string propertyName, string value)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(propertyName);
            propertyInfo?.SetValue(this, value);
        }

        public string GetValue(string propertyName)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(propertyName);
            return propertyInfo?.GetValue(this).ToString() ?? string.Empty;
        }

        private PropertyInfo GetPropertyInfo(string propertyName)
        {
            PropertyInfo propertyInfo = this.GetType().GetProperty(propertyName);

            if (propertyInfo == null)
                Errors.ErrorObservable.GetInstance().Raise($"Property {propertyName} is not a part of {this.GetType().Name}");

            return propertyInfo;
        }
    }
}