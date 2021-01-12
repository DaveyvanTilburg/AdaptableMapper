using System.Collections.Generic;
using System.Linq;
using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.DataStructure;

namespace MappingFramework.Traversals.DataStructure
{
    public sealed class DataStructureGetListValueTraversal : GetListValueTraversal, ResolvableByTypeId, GetValueTraversalPathProperty
    {
        public const string _typeId = "0e07b7e3-95bd-4d35-aa9d-e89e3c51b1b4";
        public string TypeId => _typeId;

        public DataStructureGetListValueTraversal() { }
        public DataStructureGetListValueTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public MethodResult<IEnumerable<object>> GetValues(Context context)
        {
            if (!(context.Source is TraversableDataStructure dataStructure))
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructure#12; source is not of expected type DataStructure", "error", Path, context.Source);
                return new NullMethodResult<IEnumerable<object>>();
            }

            var pathContainer = PathContainer.Create(Path);

            List<TraversableDataStructure> pathTargets = dataStructure.NavigateToAll(pathContainer.CreatePathQueue()).ToList();
            List<object> scopes = pathTargets
                .Where(m => m.IsValid())
                .Select(p => p.GetListProperty(pathContainer.LastInPath))
                .SelectMany(l => (IEnumerable<object>)l)
                .ToList();

            return new MethodResult<IEnumerable<object>>(scopes);
        }
    }
}