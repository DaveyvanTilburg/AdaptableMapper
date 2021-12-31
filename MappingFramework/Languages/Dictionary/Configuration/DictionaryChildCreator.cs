using System.Collections.Generic;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;

namespace MappingFramework.Languages.Dictionary.Configuration
{
    [ContentType(ContentType.Dictionary)]
    public sealed class DictionaryChildCreator : ChildCreator, ResolvableByTypeId
    {
        public const string _typeId = "c95700fe-4a24-4106-a746-e0682cf6b69c";
        public string TypeId => _typeId;

        public GetValueTraversal Key { get; set; }

        public DictionaryChildCreator() { }

        public DictionaryChildCreator(GetValueTraversal key)
        {
            Key = key;
        }

        public object CreateChild(Context context, Template template)
            => new Dictionary<string, object>((IDictionary<string,object>)template.Child);

        public void AddToParent(Context context, Template template, object newChild)
        {
            IDictionary<string, object> parent = ((IDictionary<string, object>)template.Parent);
            string key = Key.GetValue(context);

            if (!parent.ContainsKey(key))
                parent.Add(key, newChild);
        }
    }
}