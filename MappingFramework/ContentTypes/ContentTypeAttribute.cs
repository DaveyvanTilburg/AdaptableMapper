using System;

namespace MappingFramework.ContentTypes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContentTypeAttribute : Attribute
    {
        public ContentType ContentType { get; }
        
        public ContentTypeAttribute(ContentType contentType)
        {
            ContentType = contentType;
        }
    }
}