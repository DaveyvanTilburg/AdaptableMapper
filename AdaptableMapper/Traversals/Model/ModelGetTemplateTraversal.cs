using AdaptableMapper.Model;
using System.Collections;
using System.Collections.Generic;

namespace AdaptableMapper.Traversals.Model
{
    public sealed class ModelGetTemplateTraversal : GetTemplateTraversal
    {
        public ModelGetTemplateTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public Template Get(object target)
        {
            if (!(target is ModelBase model))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#22; target is not of expected type Model", "error", Path, target);
                return new Template 
                { 
                    Parent = new NullModel(), 
                    Child = new List<NullModel>() 
                };
            }

            var modelPathContainer = PathContainer.Create(Path);

            ModelBase pathTarget = model.GetOrCreateModel(modelPathContainer.CreatePathQueue());
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