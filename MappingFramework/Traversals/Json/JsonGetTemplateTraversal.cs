using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using Newtonsoft.Json.Linq;

namespace MappingFramework.Traversals.Json
{
    [ContentType(ContentType.Json)]
    public sealed class JsonGetTemplateTraversal : GetTemplateTraversal, ResolvableByTypeId
    {
        public const string _typeId = "afbdac94-e752-468c-84ce-e704ec988458";
        public string TypeId => _typeId;

        public JsonGetTemplateTraversal() { }
        public JsonGetTemplateTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public Template GetTemplate(Context context, object target)
        {
            JToken jToken = (JToken)target;
            JToken result = jToken.Traverse(Path, context);
            
            if (result.Type == JTokenType.Null)
            {
                context.NavigationResultIsEmpty(Path);
                return CreateNullTemplate();
            }

            if (result.Parent == null)
            {
                context.TemplatePathNeedsAParent(Path);
                return CreateNullTemplate();
            }

            if (!(result.Parent is JArray))
            {
                context.NavigationInvalid(Path, "Path should end in a node whose parent is an array", this);
                return CreateNullTemplate();
            }

            var template = new Template
            {
                Parent = result.Parent
            };

            var templateCache = context.MappingCaches.GetCache<TemplateCache>(nameof(TemplateCache));

            bool hasAccessed = templateCache.HasAccessed(Path, target);
            object storedTemplate = templateCache.GetTemplate(Path, target);
            if (storedTemplate == null)
            {
                templateCache.SetTemplate(Path, result);
                result.Remove();

                storedTemplate = result;
            }
            else if (!hasAccessed)
                result.Remove();

            template.Child = storedTemplate;

            return template;
        }

        private static Template CreateNullTemplate()
            => new Template { Parent = new JArray(), Child = new JObject() };
    }
}