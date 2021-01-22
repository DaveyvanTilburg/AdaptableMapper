using System.Collections;
using System.Collections.Generic;
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

        public Template GetTemplate(object target, MappingCaches mappingCaches)
        {
            if (!(target is TraversableDataStructure dataStructure))
            {
                Process.ProcessObservable.GetInstance().Raise("DataStructure#22; target is not of expected type DataStructure", "error", Path, target);
                return new Template 
                { 
                    Parent = new NullDataStructure(), 
                    Child = new List<NullDataStructure>() 
                };
            }

            var pathContainer = PathContainer.Create(Path);

            TraversableDataStructure pathTarget = dataStructure.GetOrCreate(pathContainer.CreatePathQueue());
            IList scopes = pathTarget.GetListProperty(pathContainer.LastInPath);

            var template = new Template
            {
                Parent = pathTarget,
                Child = scopes
            };

            return template;
        }
    }
}