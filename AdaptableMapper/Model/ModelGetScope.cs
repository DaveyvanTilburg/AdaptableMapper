using AdaptableMapper.Memory.Language;
using AdaptableMapper.Traversals;
using System.Collections;
using System.Collections.Generic;

namespace AdaptableMapper.Memory
{
    public sealed class ModelGetScope : GetScopeTraversal
    {
        public ModelGetScope(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public IEnumerable<object> GetScope(object source)
        {
            if (!(source is ModelBase adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return new List<object>();
            }

            var adaptablePathContainer = ModelPathContainer.CreateAdaptablePath(Path);

            ModelBase pathTarget = adaptable.NavigateToAdaptable(adaptablePathContainer.CreatePathQueue());
            IList adaptableScope = pathTarget.GetListProperty(adaptablePathContainer.PropertyName);

            return (IEnumerable<object>)adaptableScope;
        }
    }
}