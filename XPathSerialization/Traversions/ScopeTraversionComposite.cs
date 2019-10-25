using System.Collections.Generic;

namespace XPathSerialization.Traversions
{
    public class ScopeTraversionComposite
    {
        public List<ScopeTraversionComposite> Children { get; set; }
        public List<Map> Mappings { get; set; }

        public void Traverse(Context context)
        {
            // Get ienumerable<object> scope
            // Create target object

            //foreach object scope > copy target object
            //execute all mappings of this composite on object scope + copy target
        }
    }
}