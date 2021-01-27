using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MappingFramework.Configuration;
using Newtonsoft.Json;

namespace MappingFramework.Languages.DataStructure
{
    public abstract class TraversableDataStructure
    {
        [JsonIgnore]
        public TraversableDataStructure Parent { get; set; }

        public TraversableDataStructure GetOrCreate(Queue<string> path, Context context)
        {
            TraversableDataStructure item = this;
            if (path.Count > 0)
                item = NavigateAndCreatePath(new Queue<string>(path), context);

            return item;
        }

        private TraversableDataStructure NavigateAndCreatePath(Queue<string> path, Context context)
        {
            string step = path.Dequeue();

            PropertyInfo propertyInfo = GetPropertyInfo(step);
            if(propertyInfo == null)
            {
                context.NavigationFailed(step);
                return new NullDataStructure();
            }

            object propertyValue = propertyInfo.GetValue(this);
            if (!(propertyValue is TraversableDataStructure next))
            {
                IList propertyList = GetIListFromProperty(propertyValue, context);

                next = propertyInfo.PropertyType.CreateDataStructure(context);

                if(!(next is NullDataStructure))
                    propertyList.Add(next);
            }

            if (path.Count > 0)
                return next.NavigateAndCreatePath(path, context);

            return next;
        }

        public IList GetListProperty(string propertyName, Context context)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(propertyName);
            object propertyValue = propertyInfo?.GetValue(this);

            if (propertyValue == null)
            {
                context.NavigationFailed(propertyName);
                return new List<NullDataStructure>();
            }

            IList result = GetIListFromProperty(propertyValue, context);
            return result;
        }

        private IList GetIListFromProperty(object propertyValue, Context context)
        {
            if (!(propertyValue is IList listProperty))
            {
                context.InvalidType(propertyValue, typeof(IList));
                return new List<NullDataStructure>();
            }

            return listProperty;
        }

        public TraversableDataStructure NavigateTo(Queue<string> path, Context context)
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
                    context.NavigationFailed(step);
                    return new NullDataStructure();
                }
            }
            else if(step.TryGetObjectFilter(out DataStructureFilter filter))
            {
                IEnumerable<TraversableDataStructure> propertyValue = GetEnumerableProperty(filter.DataStructureName, context);
                next = propertyValue.FirstOrDefault(a => a.GetValue(filter.PropertyName, context).Equals(filter.Value));

                if (next == null)
                {
                    context.NavigationFailed(filter.Value);
                    return new NullDataStructure();
                }
            }
            else
            {
                IEnumerable<TraversableDataStructure> propertyValue = GetEnumerableProperty(step, context);
                next = propertyValue.FirstOrDefault();

                if (next == null)
                {
                    context.NavigationFailed(step);
                    return new NullDataStructure();
                }
            }

            if (!next.IsValid())
                return next;

            if (path.Count > 0)
                return next.NavigateTo(path, context);

            return next;
        }

        public IEnumerable<TraversableDataStructure> NavigateToAll(Queue<string> path, Context context)
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

            foreach (TraversableDataStructure item in NavigateToAll(step, context))
            {
                if(path.Count == 0)
                    yield return item;
                else
                {
                    foreach (TraversableDataStructure result in item.NavigateToAll(new Queue<string>(path), context))
                        yield return result;
                }
            }
        }

        private IEnumerable<TraversableDataStructure> NavigateToAll(string step, Context context)
        {
            if (step.TryGetObjectFilter(out DataStructureFilter filter))
            {
                IEnumerable<TraversableDataStructure> propertyValue = GetEnumerableProperty(filter.DataStructureName, context);
                foreach (TraversableDataStructure item in propertyValue.Where(a => a.GetValue(filter.PropertyName, context).Equals(filter.Value)))
                    yield return item;
            }
            else
            {
                IEnumerable<TraversableDataStructure> propertyValue = GetEnumerableProperty(step, context);

                foreach (TraversableDataStructure item in propertyValue)
                    yield return item;
            }
        }

        private IEnumerable<TraversableDataStructure> GetEnumerableProperty(string propertyName, Context context)
        {
            object property = GetProperty(propertyName, context);

            if (!(property is IEnumerable<TraversableDataStructure> enumerableProperty))
            {
                if(!(property is TraversableDataStructure item))
                {
                    context.NavigationFailed(propertyName);
                    return new List<NullDataStructure> { new NullDataStructure() };
                }

                return new List<TraversableDataStructure> { item };
            }

            return enumerableProperty;
        }
        
        private PropertyInfo GetPropertyInfo(string propertyName)
        {
            PropertyInfo propertyInfo = GetType().GetProperty(propertyName);
            return propertyInfo;
        }


        private object GetProperty(string propertyName, Context context)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(propertyName);
            object propertyValue = propertyInfo?.GetValue(this);

            if (propertyValue == null)
            {
                context.NavigationFailed(propertyName);
                return new NullDataStructure();
            }

            return propertyValue;
        }

        public void SetValue(string propertyName, string value)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(propertyName);
            propertyInfo?.SetValue(this, value);
        }

        public string GetValue(string propertyName, Context context)
        {
            object valueContainer = GetProperty(propertyName, context);

            if (valueContainer is TraversableDataStructure traversableDataStructure)
                return traversableDataStructure.IsValid() ? traversableDataStructure.ToString() : string.Empty;

            return valueContainer?.ToString() ?? string.Empty;
        }

        internal virtual bool IsValid() => true;
    }
}