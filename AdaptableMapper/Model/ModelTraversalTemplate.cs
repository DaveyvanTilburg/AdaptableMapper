using AdaptableMapper.Memory.Language;
using AdaptableMapper.Traversals;
using System.Collections;

namespace AdaptableMapper.Memory
{
    public sealed class ModelTraversalTemplate : TraversalToGetTemplate
    {
        public ModelTraversalTemplate(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public object Traverse(object target)
        {
            if (!(target is ModelBase adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return string.Empty;
            }

            var adaptablePathContainer = ModelPathContainer.CreateAdaptablePath(Path);

            ModelBase pathTarget = adaptable.GetOrCreateAdaptable(adaptablePathContainer.CreatePathQueue());
            IList adaptableScope = pathTarget.GetListProperty(adaptablePathContainer.PropertyName);

            return adaptableScope;
        }
    }
}