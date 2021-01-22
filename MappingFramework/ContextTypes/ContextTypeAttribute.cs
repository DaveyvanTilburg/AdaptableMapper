using System;

namespace MappingFramework.ContextTypes
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