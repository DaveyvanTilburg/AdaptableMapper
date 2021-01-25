using System;
using System.Collections.Generic;

namespace MappingFramework.ContentTypes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContentTypeAttribute : Attribute
    {
        public List<ContentType> ContentType { get; }
        
        public ContentTypeAttribute(params ContentType[] contentTypes)
        {
            ContentType = new List<ContentType>(contentTypes);
        }
    }
}