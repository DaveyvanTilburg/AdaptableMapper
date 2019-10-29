using AdaptableMapper.Model.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Model
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
            if (!(source is ModelBase model))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return string.Empty;
            }

            string searchValue = null;
            if (!string.IsNullOrWhiteSpace(SearchPath))
            {
                var searchModelPath = ModelPathContainer.CreateModelPath(SearchPath);

                ModelBase searchPathTarget = model.NavigateToModel(searchModelPath.CreatePathQueue());
                searchValue = searchPathTarget.GetValue(searchModelPath.PropertyName);

                if (string.IsNullOrWhiteSpace(searchValue))
                {
                    Errors.ErrorObservable.GetInstance().Raise("SearchPath resulted in empty string");
                    return string.Empty;
                }
            }

            string actualModelPath = string.IsNullOrWhiteSpace(searchValue) ? Path : Path.Replace("{{searchValue}}", searchValue);
            var modelPathContainer = ModelPathContainer.CreateModelPath(actualModelPath);

            ModelBase pathTarget = model.NavigateToModel(modelPathContainer.CreatePathQueue());
            string value = pathTarget.GetValue(modelPathContainer.PropertyName);

            return value;
        }
    }
}