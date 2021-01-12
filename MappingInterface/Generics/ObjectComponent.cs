using System;
using System.Collections.Generic;
using System.Linq;

namespace MappingFramework.MappingInterface.Generics
{
    public class ObjectComponent
    {
        private readonly object _subject;
        
        public ObjectComponent(object subject)
        {
            _subject = subject;
        }
        
        public IEnumerable<ObjectComponentLink> Requirements()
        {
            Type subjectType = _subject.GetType();
            
            IEnumerable<ObjectComponentLink> result = subjectType
                .GetProperties()
                .Where(p => !p.Name.Equals("TypeId"))
                .Select(p => new ObjectComponentLink(p, _subject));

            return result;
        }
    }
}