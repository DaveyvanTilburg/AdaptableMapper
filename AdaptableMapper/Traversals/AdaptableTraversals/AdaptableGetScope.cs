using System.Collections;
using System.Collections.Generic;

namespace AdaptableMapper.Traversals.AdaptableTraversals
{
    public class AdaptableGetScope : GetScopeTraversal
    {
        public string Path { get; set; }

        public IEnumerable<object> GetScope(object source)
        {
            if (!(source is Adaptable adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Adaptable");
                return new List<object>();
            }

            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(Path);

            Adaptable pathTarget = adaptable.NavigateToAdaptable(adaptablePathContainer.CreatePathQueue());
            IList adaptableScope = pathTarget.GetListProperty(adaptablePathContainer.PropertyName);

            return (IEnumerable<object>)adaptableScope;
        }
    }
}