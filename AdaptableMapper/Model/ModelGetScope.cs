using AdaptableMapper.Model.Language;
using AdaptableMapper.Traversals;
using System.Collections;
using System.Collections.Generic;

namespace AdaptableMapper.Model
{
    public sealed class ModelGetScope : GetScopeTraversal
    {
        public ModelGetScope(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public IEnumerable<object> GetScope(object source)
        {
            if (!(source is ModelBase model))
            {
                Errors.ErrorObservable.GetInstance().Raise("Object is not of expected type Model");
                return new List<object>();
            }

            var modelPathContainer = ModelPathContainer.CreateModelPath(Path);

            ModelBase pathTarget = model.NavigateToModel(modelPathContainer.CreatePathQueue());
            IList modelScope = pathTarget.GetListProperty(modelPathContainer.PropertyName);

            return (IEnumerable<object>)modelScope;
        }
    }
}