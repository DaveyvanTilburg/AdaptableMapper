using System.Collections.Generic;
using System.Linq;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.DataStructure;

namespace MappingFramework.Traversals.DataStructure
{
    [ContentType(ContentType.DataStructure)]
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
            TraversableDataStructure dataStructure = (TraversableDataStructure)context.Source;

            var pathContainer = PathContainer.Create(Path);

            List<TraversableDataStructure> pathTargets = dataStructure.NavigateToAll(pathContainer.CreatePathQueue(), context).ToList();
            List<object> scopes = pathTargets
                .Where(m => m.IsValid())
                .Select(p => p.GetListProperty(pathContainer.LastInPath, context))
                .SelectMany(l => (IEnumerable<object>)l)
                .ToList();

            return new MethodResult<IEnumerable<object>>(scopes);
        }
    }
}