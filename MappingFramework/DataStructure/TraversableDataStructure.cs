using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MappingFramework.Process;

namespace MappingFramework.DataStructure
{
    public abstract class TraversableDataStructure
    {
        [JsonIgnore]
        public TraversableDataStructure Parent { get; set; }

        public TraversableDataStructure GetOrCreate(Queue<string> path)
        {
            TraversableDataStructure item = this;
            if (path.Count > 0)
                item = NavigateAndCreatePath(new Queue<string>(path));

            return item;
        }

        private TraversableDataStructure NavigateAndCreatePath(Queue<string> path)
        {
            string step = path.Dequeue();

            PropertyInfo propertyInfo = GetPropertyInfo(step);
            if(propertyInfo == null)
            {
                ProcessObservable.GetInstance().Raise($"DataStructure#1; No property with name {step} found on type {this.GetType().Name}", "warning");
                return new NullDataStructure();
            }

            object propertyValue = propertyInfo.GetValue(this);
            if (!(propertyValue is TraversableDataStructure next))
            {
                IList propertyList = GetIListFromProperty(propertyValue);

                next = propertyInfo.PropertyType.CreateDataStructure();

                if(!(next is NullDataStructure))
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
                ProcessObservable.GetInstance().Raise($"DataStructure#7; No property with name {propertyName} found on type {this.GetType().Name}", "warning");
                return new List<NullDataStructure>();
            }

            IList result = GetIListFromProperty(propertyValue);
            return result;
        }

        private IList GetIListFromProperty(object propertyValue)
        {
            if (!(propertyValue is IList listProperty))
            {
                ProcessObservable.GetInstance().Raise("DataStructure#2; Property is not traversable, it is not a list of DataStructure", "warning");
                return new List<NullDataStructure>();
            }

            return listProperty;
        }

        public TraversableDataStructure NavigateTo(Queue<string> path)
        {
            if (path.Count == 0)
                return this;

            string step = path.Dequeue();

            TraversableDataStructure next;
            if (step.Equals(".."))
            {
                next = Parent;
                if (next == null)
                {
                    ProcessObservable.GetInstance().Raise($"DataStructure#3; Parent node was null while navigating to parent of type {this.GetType().Name}", "warning");
                    return new NullDataStructure();
                }
            }
            else if(step.TryGetObjectFilter(out DataStructureFilter filter))
            {
                IEnumerable<TraversableDataStructure> propertyValue = GetEnumerableProperty(filter.DataStructureName);
                next = propertyValue.FirstOrDefault(a => a.GetValue(filter.PropertyName).Equals(filter.Value));

                if (next == null)
                {
                    ProcessObservable.GetInstance().Raise($"DataStructure#4; No match found for filter on list with name {filter.DataStructureName} with a value that has a {filter.PropertyName} with value {filter.Value}", "warning");
                    return new NullDataStructure();
                }
            }
            else
            {
                IEnumerable<TraversableDataStructure> propertyValue = GetEnumerableProperty(step);
                next = propertyValue.FirstOrDefault();

                if (next == null)
                {
                    ProcessObservable.GetInstance().Raise($"DataStructure#5; No items found in {step} in type {this.GetType().Name}", "warning");
                    return new NullDataStructure();
                }
            }

            if (!next.IsValid())
                return next;

            if (path.Count > 0)
                return next.NavigateTo(path);

            return next;
        }

        public IEnumerable<TraversableDataStructure> NavigateToAll(Queue<string> path)
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

            foreach (TraversableDataStructure item in NavigateToAll(step))
            {
                if(path.Count == 0)
                    yield return item;
                else
                {
                    foreach (TraversableDataStructure result in item.NavigateToAll(new Queue<string>(path)))
                        yield return result;
                }
            }
        }

        private IEnumerable<TraversableDataStructure> NavigateToAll(string step)
        {
            if (step.TryGetObjectFilter(out DataStructureFilter filter))
            {
                IEnumerable<TraversableDataStructure> propertyValue = GetEnumerableProperty(filter.DataStructureName);
                foreach (TraversableDataStructure item in propertyValue.Where(a => a.GetValue(filter.PropertyName).Equals(filter.Value)))
                    yield return item;
            }
            else
            {
                IEnumerable<TraversableDataStructure> propertyValue = GetEnumerableProperty(step);

                foreach (TraversableDataStructure item in propertyValue)
                    yield return item;
            }
        }

        private IEnumerable<TraversableDataStructure> GetEnumerableProperty(string propertyName)
        {
            object property = GetProperty(propertyName);

            if (!(property is IEnumerable<TraversableDataStructure> enumerableProperty))
            {
                if(!(property is TraversableDataStructure item))
                {
                    ProcessObservable.GetInstance().Raise($"DataStructure#8; Property {propertyName} on type {this.GetType().Name} is not traversable, it is not a list of TraversableDataStructure or a derivative of TraversableDataStructure", "error");
                    return new List<NullDataStructure> { new NullDataStructure() };
                }

                return new List<TraversableDataStructure> { item };
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
                ProcessObservable.GetInstance().Raise($"DataStructure#9; Property {propertyName} is not a part of {this.GetType().Name}", "warning");
                return new NullDataStructure();
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

            if (valueContainer is TraversableDataStructure traversableDataStructure)
                return traversableDataStructure.IsValid() ? traversableDataStructure.ToString() : string.Empty;

            return valueContainer?.ToString() ?? string.Empty;
        }

        internal virtual bool IsValid()
        {
            return true;
        }
    }
}