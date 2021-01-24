using System.Collections.Generic;
using MappingFramework.Process;

namespace MappingFramework
{
    public class MapResult
    {
        public MapResult(object result, IEnumerable<Information> information)
        {
            Result = result;
            Information = new List<Information>(information);
        }
        
        public object Result { get; }
        public List<Information> Information { get; }
    }
}