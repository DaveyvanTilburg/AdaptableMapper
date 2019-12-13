using Newtonsoft.Json.Linq;

namespace AdaptableMapper.Traversals.Json
{
    public sealed class JsonGetTemplateTraversal : GetTemplateTraversal
    {
        public JsonGetTemplateTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public Template GetTemplate(object target, MappingCaches mappingCaches)
        {
            if (!(target is JToken jToken))
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#23; target is not of expected type jToken", "error", Path, target?.GetType().Name);
                return CreateNullTemplate();
            }

            JToken result = jToken.Traverse(Path);
            if (result.Type == JTokenType.Null)
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#24; Path resulted in no items", "warning", Path, target);
                return CreateNullTemplate();
            }

            if (result.Parent == null)
            {
                Process.ProcessObservable.GetInstance().Raise("JSON#9; Path resulted in an item that has no parent", "error", Path, target);
                return CreateNullTemplate();
            }

            var template = new Template
            {
                Parent = result.Parent
            };

            var templateCache = mappingCaches.GetCache<TemplateCache>(nameof(TemplateCache));

            bool hasAccessed = templateCache.HasAccessed(Path, target);
            object storedTemplate = templateCache.GetTemplate(Path, target);
            if (storedTemplate == null)
            {
                templateCache.SetTemplate(Path, result);
                result.Remove();

                storedTemplate = result;
            }
            else if (!hasAccessed)
            {
                result.Remove();
            }

            template.Child = storedTemplate;

            return template;
        }

        private static Template CreateNullTemplate()
        {
            return new Template { Parent = new JObject(), Child = new JObject() };
        }
    }
}