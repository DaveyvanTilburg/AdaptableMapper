using AdaptableMapper.Model.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Model
{
    public sealed class ModelGetValue : GetValueTraversal
    {
        public ModelGetValue(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public string GetValue(object source)
        {
            if(!(source is ModelBase model))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#16; source is not of expected type Model", "error", Path, source);
                return string.Empty;
            }

            var modelPathContainer = PathContainer.Create(Path);

            ModelBase pathTarget = model.NavigateToModel(modelPathContainer.CreatePathQueue());
            string value = pathTarget.GetValue(modelPathContainer.LastInPath);

            return value;
        }
    }
}