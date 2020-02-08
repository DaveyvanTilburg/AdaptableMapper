using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MappingFramework.Model
{
    public abstract class ModelBase
    {
        [JsonIgnore]
        public ModelBase Parent { get; set; }

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

            PropertyInfo propertyInfo = GetPropertyInfo(step);
            if(propertyInfo == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"MODEL#1; No property with name {step} found on type {this.GetType().Name}", "warning");
                return new NullModel();
            }

            object propertyValue = propertyInfo.GetValue(this);
            if (!(propertyValue is ModelBase next))
            {
                IList propertyList = GetIListFromProperty(propertyValue);

                next = propertyInfo.PropertyType.CreateModel();

                if(!(next is NullModel))
                    propertyList.Add(next);
            }

            if (path.Count > 0)
                return next.NavigateAndCreatePath(path);

            return next;
        }

        public IList GetListProperty(string propertyName)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(propertyName);
            object propertyValue = propertyInfo?.GetValue(this);

            if (propertyValue == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"MODEL#7; No property with name {propertyName} found on type {this.GetType().Name}", "warning");
                return new List<NullModel>();
            }

            IList result = GetIListFromProperty(propertyValue);
            return result;
        }

        private IList GetIListFromProperty(object propertyValue)
        {
            if (!(propertyValue is IList listProperty))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#2; Property is not traversable, it is not a list of Model", "warning");
                return new List<NullModel>();
            }

            return listProperty;
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
                {
                    Process.ProcessObservable.GetInstance().Raise($"MODEL#3; Parent node was null while navigating to parent of type {this.GetType().Name}", "warning");
                    return new NullModel();
                }
            }
            else if(step.TryGetObjectFilter(out ModelFilter filter))
            {
                IEnumerable<ModelBase> propertyValue = GetEnumerableProperty(filter.ModelName);
                next = propertyValue.FirstOrDefault(a => a.GetValue(filter.PropertyName).Equals(filter.Value));

                if (next == null)
                {
                    Process.ProcessObservable.GetInstance().Raise($"MODEL#4; No match found for filter on list with name {filter.ModelName} with a value that has a {filter.PropertyName} with value {filter.Value}", "warning");
                    return new NullModel();
                }
            }
            else
            {
                IEnumerable<ModelBase> propertyValue = GetEnumerableProperty(step);
                next = propertyValue.FirstOrDefault();

                if (next == null)
                {
                    Process.ProcessObservable.GetInstance().Raise($"MODEL#5; No items found in {step} in type {this.GetType().Name}", "warning");
                    return new NullModel();
                }
            }

            if (!next.IsValid())
                return next;

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

            if (string.IsNullOrWhiteSpace(step))
            {
                yield return this;
                yield break;
            }

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
            if (step.TryGetObjectFilter(out ModelFilter filter))
            {
                IEnumerable<ModelBase> propertyValue = GetEnumerableProperty(filter.ModelName);
                foreach (ModelBase modelBase in propertyValue.Where(a => a.GetValue(filter.PropertyName).Equals(filter.Value)))
                    yield return modelBase;
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
            object property = GetProperty(propertyName);

            if (!(property is IEnumerable<ModelBase> enumerableProperty))
            {
                if(!(property is ModelBase modelBase))
                {
                    Process.ProcessObservable.GetInstance().Raise($"MODEL#8; Property {propertyName} on type {this.GetType().Name} is not traversable, it is not a list of ModelBase or a derivative of ModelBase", "error");
                    return new List<NullModel> { new NullModel() };
                }

                return new List<ModelBase> { modelBase };
            }

            return enumerableProperty;
        }
        private PropertyInfo GetPropertyInfo(string propertyName)
        {
            PropertyInfo propertyInfo = this.GetType().GetProperty(propertyName);
            return propertyInfo;
        }


        private object GetProperty(string propertyName)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(propertyName);
            object propertyValue = propertyInfo?.GetValue(this);

            if (propertyValue == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"MODEL#9; Property {propertyName} is not a part of {this.GetType().Name}", "warning");
                return new NullModel();
            }

            return propertyValue;
        }

        public void SetValue(string propertyName, string value)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(propertyName);
            propertyInfo?.SetValue(this, value);
        }

        public string GetValue(string propertyName)
        {
            object valueContainer = GetProperty(propertyName);

            if (valueContainer is ModelBase modelBase)
                return modelBase.IsValid() ? modelBase.ToString() : string.Empty;

            return valueContainer?.ToString() ?? string.Empty;
        }

        internal virtual bool IsValid()
        {
            return true;
        }
    }
}