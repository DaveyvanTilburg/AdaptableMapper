using AdaptableMapper.Model;
using System.Collections.Generic;
using System.Linq;
using AdaptableMapper.Converters;

namespace AdaptableMapper.Traversals.Model
{
    public sealed class ModelGetListValueTraversal : GetListValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "0e07b7e3-95bd-4d35-aa9d-e89e3c51b1b4";
        public string TypeId => _typeId;

        public ModelGetListValueTraversal() { }
        public ModelGetListValueTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public MethodResult<IEnumerable<object>> GetValues(object source)
        {
            if (!(source is ModelBase model))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#12; source is not of expected type Model", "error", Path, source);
                return new NullMethodResult<IEnumerable<object>>();
            }

            var modelPathContainer = PathContainer.Create(Path);

            List<ModelBase> pathTargets = model.NavigateToAllModels(modelPathContainer.CreatePathQueue()).ToList();
            List<object> modelScopes = pathTargets
                .Where(m => m.IsValid())
                .Select(p => p.GetListProperty(modelPathContainer.LastInPath))
                .SelectMany(l => (IEnumerable<object>)l)
                .ToList();

            return new MethodResult<IEnumerable<object>>(modelScopes);
        }
    }
}