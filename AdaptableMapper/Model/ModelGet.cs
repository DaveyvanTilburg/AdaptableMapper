using AdaptableMapper.Model.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Model
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
            if(!(source is ModelBase model))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return string.Empty;
            }

            var modelPathContainer = ModelPathContainer.CreateModelPath(Path);

            ModelBase pathTarget = model.NavigateToModel(modelPathContainer.CreatePathQueue());
            string value = pathTarget.GetValue(modelPathContainer.PropertyName);

            return value;
        }
    }
}