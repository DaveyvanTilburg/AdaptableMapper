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
                Errors.ProcessObservable.GetInstance().Raise("MODEL#13; source is not of expected type Model", "error", SearchPath, SearchValuePath, source);
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
                    Errors.ProcessObservable.GetInstance().Raise("MODEL#14; SearchPath resulted in empty string", "warning", SearchPath, SearchValuePath, source);
                    return string.Empty;
                }
            }

            string actualPath = string.IsNullOrWhiteSpace(searchValue) ? SearchPath : SearchPath.Replace("{{searchValue}}", searchValue);
            var modelPathContainer = PathContainer.Create(actualPath);

            ModelBase pathTarget = model.NavigateToModel(modelPathContainer.CreatePathQueue());
            if (pathTarget == null)
            {
                Errors.ProcessObservable.GetInstance().Raise("MODEL#15; ActualPath resulted in no items", "warning", actualPath, SearchPath, SearchValuePath, source);
                return string.Empty;
            }

            string searchResult = pathTarget.GetValue(modelPathContainer.LastInPath);
            return searchResult;
        }
    }
}