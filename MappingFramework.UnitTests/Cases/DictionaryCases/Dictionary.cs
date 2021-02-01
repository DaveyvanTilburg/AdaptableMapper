using System;
using System.Collections.Generic;

namespace MappingFramework.UnitTests.Cases.DictionaryCases
{
    internal class Dictionary
    {
        public static object CreateTarget(ContextType contextType)
        {
            object result = null;

            switch (contextType)
            {
                case ContextType.EmptyObject:
                    break;
                case ContextType.TestObject:
                    result = new Dictionary<string, object>();
                    break;
                case ContextType.InvalidObject:
                    result = DateTime.MinValue;
                    break;
            }

            return result;
        }
    }
}