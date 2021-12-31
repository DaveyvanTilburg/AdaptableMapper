using System.Collections.Generic;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;

namespace MappingFramework.Languages.Dictionary.Configuration
{
    [ContentType(ContentType.Dictionary)]
    public sealed class DictionaryChildToParent : ChildCreator, ResolvableByTypeId
    {
        public const string _typeId = "51bc0a8e-45e4-4c75-b338-99b3b9b91e97";
        public string TypeId => _typeId;

        public DictionaryChildToParent() { }

        public object CreateChild(Context context, Template template)
            => (IDictionary<string, object>)template.Parent;

        public void AddToParent(Context context, Template template, object newChild)
        {
            
        }
    }
}