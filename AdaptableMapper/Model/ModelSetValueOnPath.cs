using AdaptableMapper.Model.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Model
{
    public sealed class ModelSetValueOnPath : SetValueTraversal
    {
        public ModelSetValueOnPath(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public void SetValue(object target, string value)
        {
            if (!(target is ModelBase model))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return;
            }

            var modelPathContainer = ModelPathContainer.CreateModelPath(Path);
            ModelBase pathTarget = model.GetOrCreateModel(modelPathContainer.CreatePathQueue());

            pathTarget.SetValue(modelPathContainer.PropertyName, value);
        }
    }
}