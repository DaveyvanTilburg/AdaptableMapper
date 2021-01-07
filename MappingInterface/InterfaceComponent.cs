using System;
using System.Collections.Generic;
using System.Linq;

namespace MappingFramework.MappingInterface
{
    public class InterfaceComponent
    {
        private readonly object _subject;
        
        public InterfaceComponent(object subject)
        {
            _subject = subject;
        }
        
        public IEnumerable<InterfaceRequirement> Requirements()
        {
            Type subjectType = _subject.GetType();
            
            IEnumerable<InterfaceRequirement> result = subjectType
                .GetProperties()
                .Where(p => !p.Name.Equals("TypeId"))
                .Select(p => new InterfaceRequirement(p));

            return result;
        }
    }
}