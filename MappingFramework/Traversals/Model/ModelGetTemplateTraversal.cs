using System.Collections;
using System.Collections.Generic;
using MappingFramework.Converters;
using MappingFramework.DataStructure;

namespace MappingFramework.Traversals.Model
{
    public sealed class ModelGetTemplateTraversal : GetTemplateTraversal, ResolvableByTypeId
    {
        public const string _typeId = "e61aee0c-d8c9-4429-8c4b-d0f3fd63d72b";
        public string TypeId => _typeId;

        public ModelGetTemplateTraversal() { }
        public ModelGetTemplateTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public Template GetTemplate(object target, MappingCaches mappingCaches)
        {
            if (!(target is TraversableDataStructure model))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#22; target is not of expected type Model", "error", Path, target);
                return new Template 
                { 
                    Parent = new NullDataStructure(), 
                    Child = new List<NullDataStructure>() 
                };
            }

            var modelPathContainer = PathContainer.Create(Path);

            TraversableDataStructure pathTarget = model.GetOrCreate(modelPathContainer.CreatePathQueue());
            IList modelScope = pathTarget.GetListProperty(modelPathContainer.LastInPath);

            var template = new Template
            {
                Parent = pathTarget,
                Child = modelScope
            };

            return template;
        }
    }
}