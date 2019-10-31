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
                Errors.ErrorObservable.GetInstance().Raise("MODEL#22; target is not of expected type Model", Path, target);
                return string.Empty;
            }

            var modelPathContainer = PathContainer.Create(Path);

            ModelBase pathTarget = model.GetOrCreateModel(modelPathContainer.CreatePathQueue());
            IList modelScope = pathTarget.GetListProperty(modelPathContainer.LastInPath);

            return modelScope;
        }
    }
}