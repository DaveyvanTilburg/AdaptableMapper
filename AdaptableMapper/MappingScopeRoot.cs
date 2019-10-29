using AdaptableMapper.Contexts;
using System.Collections.Generic;

namespace AdaptableMapper
{
    public sealed class MappingScopeRoot : MappingScope
    {
        public List<MappingScopeComposite> MappingScopeComposite { get; set; }

        public MappingScopeRoot(
            List<MappingScopeComposite> mappingScopeComposite)
        {
            MappingScopeComposite = mappingScopeComposite;
        }

        void MappingScope.Traverse(Context context)
        {
            foreach (MappingScopeComposite child in MappingScopeComposite)
                child.Traverse(context);
        }
    }
}