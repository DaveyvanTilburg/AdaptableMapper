using AdaptableMapper.Memory.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Memory
{
    public class AdaptableGetSearch : GetTraversal
    {
        public AdaptableGetSearch(string path, string searchPath)
        {
            Path = path;
            SearchPath = searchPath;
        }

        public string Path { get; set; }
        public string SearchPath { get; set; }

        public string GetValue(object source)
        {
            if (!(source is Adaptable adaptable))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Adaptable");
                return string.Empty;
            }

            string searchValue = null;
            if (!string.IsNullOrWhiteSpace(SearchPath))
            {
                var searchAdaptablePath = AdaptablePathContainer.CreateAdaptablePath(SearchPath);

                Adaptable searchPathTarget = adaptable.NavigateToAdaptable(searchAdaptablePath.CreatePathQueue());
                searchValue = searchPathTarget.GetValue(searchAdaptablePath.PropertyName);
            }

            string actualAdaptablePath = string.IsNullOrWhiteSpace(searchValue) ? Path : Path.Replace("{{searchResult}}", searchValue);
            var adaptablePathContainer = AdaptablePathContainer.CreateAdaptablePath(actualAdaptablePath);

            Adaptable pathTarget = adaptable.NavigateToAdaptable(adaptablePathContainer.CreatePathQueue());
            string value = pathTarget.GetValue(adaptablePathContainer.PropertyName);

            return value;
        }
    }
}