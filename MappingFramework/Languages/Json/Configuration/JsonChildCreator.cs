using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;
using Newtonsoft.Json.Linq;

namespace MappingFramework.Languages.Json.Configuration
{
    [ContentType(ContentType.Json)]
    public class JsonChildCreator : ChildCreator, ResolvableByTypeId
    {
        public const string _typeId = "88a88d4e-b79e-471a-9866-04a217e2e890";
        public string TypeId => _typeId;

        public JsonChildCreator() { }

        public object CreateChild(Context context, Template template)
            => ((JToken)template.Child).DeepClone();

        public void AddToParent(Context context, Template template, object newChild)
            => ((JArray)template.Parent).Add((JToken)newChild);
    }
}