using System.Collections;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.DataStructure;

namespace MappingFramework.Traversals.DataStructure
{
    [ContentType(ContentType.DataStructure)]
    public sealed class DataStructureGetTemplateTraversal : GetTemplateTraversal, ResolvableByTypeId
    {
        public const string _typeId = "e61aee0c-d8c9-4429-8c4b-d0f3fd63d72b";
        public string TypeId => _typeId;

        public DataStructureGetTemplateTraversal() { }
        public DataStructureGetTemplateTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public Template GetTemplate(Context context, object target, MappingCaches mappingCaches)
        {
            TraversableDataStructure dataStructure = (TraversableDataStructure)target;

            var pathContainer = PathContainer.Create(Path);

            TraversableDataStructure pathTarget = dataStructure.GetOrCreate(pathContainer.CreatePathQueue(), context);
            IList scopes = pathTarget.GetListProperty(pathContainer.LastInPath, context);

            var template = new Template
            {
                Parent = pathTarget,
                Child = scopes
            };

            return template;
        }
    }
}