using System;

namespace MappingFramework
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class ContextTypeAttribute : Attribute
    {
        public ContextType ContextType { get; }
        
        public ContextTypeAttribute(ContextType contextType)
        {
            ContextType = contextType;
        }
    }
}