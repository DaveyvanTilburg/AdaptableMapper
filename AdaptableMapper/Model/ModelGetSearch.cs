using AdaptableMapper.Memory.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Memory
{
    public sealed class ModelGetSearch : GetValueTraversal
    {
        public ModelGetSearch(string path, string searchPath)
        {
            Path = path;
            SearchPath = searchPath;
        }

        public string Path { get; set; }
        public string SearchPath { get; set; }

        public string GetValue(object source)
        {
            if (!(source is ModelBase adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return string.Empty;
            }

            string searchValue = null;
            if (!string.IsNullOrWhiteSpace(SearchPath))
            {
                var searchAdaptablePath = ModelPathContainer.CreateAdaptablePath(SearchPath);

                ModelBase searchPathTarget = adaptable.NavigateToAdaptable(searchAdaptablePath.CreatePathQueue());
                searchValue = searchPathTarget.GetValue(searchAdaptablePath.PropertyName);
            }

            string actualAdaptablePath = string.IsNullOrWhiteSpace(searchValue) ? Path : Path.Replace("{{searchResult}}", searchValue);
            var adaptablePathContainer = ModelPathContainer.CreateAdaptablePath(actualAdaptablePath);

            ModelBase pathTarget = adaptable.NavigateToAdaptable(adaptablePathContainer.CreatePathQueue());
            string value = pathTarget.GetValue(adaptablePathContainer.PropertyName);

            return value;
        }
    }
}