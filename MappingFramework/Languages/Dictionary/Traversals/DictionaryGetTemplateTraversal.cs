using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;

namespace MappingFramework.Languages.Dictionary.Traversals
{

    [ContentType(ContentType.Dictionary)]
    public sealed class DictionaryGetTemplateTraversal : GetTemplateTraversal, ResolvableByTypeId
    {
        public const string _typeId = "0a7c1f55-1f97-4f47-834c-718ce11325c7";
        public string TypeId => _typeId;

        public DictionaryGetTemplateTraversal() { }

        public Template GetTemplate(Context context, object target)
        {
            var template = new Template
            {
                Parent = target,
                Child = new System.Collections.Generic.Dictionary<string, object>()
            };

            return template;
        }
    }
}