using AdaptableMapper.Model.Language;
using AdaptableMapper.Traversals;
using System.Collections;

namespace AdaptableMapper.Model
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
            if (!(target is ModelBase model))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return string.Empty;
            }

            var modelPathContainer = ModelPathContainer.CreateModelPath(Path);

            ModelBase pathTarget = model.GetOrCreateModel(modelPathContainer.CreatePathQueue());
            IList modelScope = pathTarget.GetListProperty(modelPathContainer.PropertyName);

            return modelScope;
        }
    }
}