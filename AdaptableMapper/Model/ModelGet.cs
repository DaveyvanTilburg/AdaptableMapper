using AdaptableMapper.Memory.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Memory
{
    public sealed class ModelGet : GetValueTraversal
    {
        public ModelGet(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public string GetValue(object source)
        {
            if(!(source is ModelBase adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return string.Empty;
            }

            var adaptablePathContainer = ModelPathContainer.CreateAdaptablePath(Path);

            ModelBase pathTarget = adaptable.NavigateToAdaptable(adaptablePathContainer.CreatePathQueue());
            string value = pathTarget.GetValue(adaptablePathContainer.PropertyName);

            return value;
        }
    }
}