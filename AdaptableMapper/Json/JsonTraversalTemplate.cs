using AdaptableMapper.Traversals;
using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Json
{
    public sealed class JsonTraversalTemplate : GetTemplate
    {
        public JsonTraversalTemplate(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public Template Get(object target)
        {
            if (!(target is JToken jToken))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#23; target is not of expected type jToken", "error", Path, target?.GetType().Name);
                return CreateNullTemplate();
            }

            JToken result = jToken.Traverse(Path);
            if (string.IsNullOrWhiteSpace(result.Path))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#24; Path resulted in no items", "warning", Path, target);
                return CreateNullTemplate();
            }

            var template = new Template 
            {
                Parent = result.Parent, 
                Child = result
            };

            result.Remove();

            return template;
        }

        private static Template CreateNullTemplate()
        {
            return new Template { Parent = new JObject(), Child = new JObject() };
        }
    }
}