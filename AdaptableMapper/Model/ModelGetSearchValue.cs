using AdaptableMapper.Model.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Model
{
    public sealed class ModelGetSearchValue : GetValueTraversal
    {
        public ModelGetSearchValue(string searchPath, string searchValuePath)
        {
            SearchPath = searchPath;
            SearchValuePath = searchValuePath;
        }

        public string SearchPath { get; set; }
        public string SearchValuePath { get; set; }

        public string GetValue(object source)
        {
            if (!(source is ModelBase model))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return string.Empty;
            }

            string searchValue = null;
            if (!string.IsNullOrWhiteSpace(SearchValuePath))
            {
                var searchModelPath = PathContainer.Create(SearchValuePath);

                ModelBase searchPathTarget = model.NavigateToModel(searchModelPath.CreatePathQueue());
                searchValue = searchPathTarget.GetValue(searchModelPath.LastInPath);

                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    Errors.ErrorObservable.GetInstance().Raise("SearchPath resulted in empty string");
                    return string.Empty;
                }
            }

            string actualModelPath = string.IsNullOrWhiteSpace(searchValue) ? SearchPath : SearchPath.Replace("{{searchValue}}", searchValue);
            var modelPathContainer = PathContainer.Create(actualModelPath);

            ModelBase pathTarget = model.NavigateToModel(modelPathContainer.CreatePathQueue());
            string searchResult = pathTarget.GetValue(modelPathContainer.LastInPath);

            return searchResult;
        }
    }
}