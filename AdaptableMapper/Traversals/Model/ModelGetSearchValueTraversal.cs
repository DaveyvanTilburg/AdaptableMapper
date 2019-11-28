using AdaptableMapper.Model;

namespace AdaptableMapper.Traversals.Model
{
    public sealed class ModelGetSearchValueTraversal : GetValueTraversal
    {
        public ModelGetSearchValueTraversal(string searchPath, string searchValuePath)
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
                Process.ProcessObservable.GetInstance().Raise("MODEL#13; source is not of expected type Model", "error", SearchPath, SearchValuePath, source);
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(SearchValuePath))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#21; SearchValuePath is empty. If this is intentional, please use ModelGetValue instead.", "error", SearchPath, SearchValuePath);
                return string.Empty;
            }

            var searchModelPath = PathContainer.Create(SearchValuePath);
            ModelBase searchPathTarget = model.NavigateToModel(searchModelPath.CreatePathQueue());
            if (!searchPathTarget.IsValid())
                return string.Empty;

            string searchValue = searchPathTarget.GetValue(searchModelPath.LastInPath);
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#14; SearchPath resulted in empty string", "warning", SearchPath, SearchValuePath, source);
                return string.Empty;
            }

            string actualPath = string.IsNullOrWhiteSpace(searchValue) ? SearchPath : SearchPath.Replace("{{searchValue}}", searchValue);
            var modelPathContainer = PathContainer.Create(actualPath);

            ModelBase pathTarget = model.NavigateToModel(modelPathContainer.CreatePathQueue());
            if (!pathTarget.IsValid())
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#15; ActualPath resulted in no items", "warning", actualPath, SearchPath, SearchValuePath, source);
                return string.Empty;
            }

            string searchResult = pathTarget.GetValue(modelPathContainer.LastInPath);
            return searchResult;
        }
    }
}