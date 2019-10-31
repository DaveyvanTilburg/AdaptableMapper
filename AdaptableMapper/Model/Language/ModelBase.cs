using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdaptableMapper.Model.Language
{
    public abstract class ModelBase
    {
        [JsonIgnore]
        public ModelBase Parent { get; set; }

        public ModelBase() { }

        public ModelBase GetOrCreateModel(Queue<string> path)
        {
            ModelBase model = this;
            if (path.Count > 0)
                model = NavigateAndCreatePath(new Queue<string>(path));

            return model;
        }

        private ModelBase NavigateAndCreatePath(Queue<string> path)
        {
            string step = path.Dequeue();

            PropertyInfo property = GetPropertyInfo(step);
            IList propertyValue = GetIListFromProperty(property, step);

            ModelBase next = property.PropertyType.CreateModel();
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
            object property = propertyInfo?.GetValue(this);

            if (property == null)
                Errors.ErrorObservable.GetInstance().Raise($"MODEL#1; Property {propertyName} does not exist on {this.GetType().Name}");

            if ((property is IList listProperty))
                return listProperty;
            else
            {
                Errors.ErrorObservable.GetInstance().Raise($"MODEL#2; Property {propertyName} is not traversable, it is not a list of Model");
                return new List<ModelBase>();
            }
        }

        public ModelBase NavigateToModel(Queue<string> path)
        {
            if (path.Count == 0)
                return this;

            string step = path.Dequeue();

            ModelBase next;
            if (step.Equals(".."))
            {
                next = Parent;
                if (next == null)
                    Errors.ErrorObservable.GetInstance().Raise($"MODEL#3; Parent node was null while navigating to parent of type {this.GetType().Name}");
            }
            else if(step.TryGetObjectFilter(out ModelFilter filter))
            {
                IEnumerable<ModelBase> propertyValue = GetEnumerableProperty(filter.ModelName);
                next = propertyValue.FirstOrDefault(a => a.GetValue(filter.PropertyName).Equals(filter.Value));

                if (next == null)
                    Errors.ErrorObservable.GetInstance().Raise($"MODEL4#; No match found for filter on list with name {filter.ModelName} with a value that has a {filter.PropertyName} with value {filter.Value}");
            }
            else
            {
                IEnumerable<ModelBase> propertyValue = GetEnumerableProperty(step);
                next = propertyValue.FirstOrDefault();

                if (next == null)
                    Errors.ErrorObservable.GetInstance().Raise($"MODEL#5; No items found in {step} in type {this.GetType().Name}");
            }

            if (next == null)
                return this;

            if (path.Count > 0)
                return next.NavigateToModel(path);

            return next;
        }

        public IEnumerable<ModelBase> NavigateToAllModels(Queue<string> path)
        {
            if(path.Count == 0)
            {
                yield return this;
                yield break;
            }

            string step = path.Dequeue();

            foreach (ModelBase modelBase in NavigateToAllModels(step))
            {
                if(path.Count == 0)
                    yield return modelBase;
                else
                {
                    foreach (ModelBase result in modelBase.NavigateToAllModels(new Queue<string>(path)))
                        yield return result;
                }
            }
        }

        private IEnumerable<ModelBase> NavigateToAllModels(string step)
        {
            if (step.Equals(".."))
            {
                if(Parent == null)
                {
                    Errors.ErrorObservable.GetInstance().Raise($"MODEL#6; Parent node was null while navigating to parent of type {this.GetType().Name}");
                    yield break;
                }

                yield return Parent;
                yield break;
            }
            else if (step.TryGetObjectFilter(out ModelFilter filter))
            {
                IEnumerable<ModelBase> propertyValue = GetEnumerableProperty(filter.ModelName);
                foreach (ModelBase modelBase in propertyValue.Where(a => a.GetValue(filter.PropertyName).Equals(filter.Value)))
                    yield return modelBase;

                yield break;
            }
            else
            {
                IEnumerable<ModelBase> propertyValue = GetEnumerableProperty(step);

                foreach (ModelBase modelBase in propertyValue)
                    yield return modelBase;
            }
        }

        private IEnumerable<ModelBase> GetEnumerableProperty(string propertyName)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(propertyName);
            object property = propertyInfo?.GetValue(this);

            if(property == null)
                Errors.ErrorObservable.GetInstance().Raise($"MODEL#7; Property {propertyName} does not exist on {this.GetType().Name}");

            if ((property is IEnumerable<ModelBase> enumerableProperty))
            {
                return enumerableProperty;
            }
            else
            {
                Errors.ErrorObservable.GetInstance().Raise($"MODEL#8; Property {propertyName} on type {this.GetType().Name} is not traversable, it is not a list of Model");
                return new List<ModelBase>();
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
                Errors.ErrorObservable.GetInstance().Raise($"MODEL#9; Property {propertyName} is not a part of {this.GetType().Name}");

            return propertyInfo;
        }
    }
}