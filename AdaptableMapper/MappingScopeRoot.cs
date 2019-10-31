using AdaptableMapper.Contexts;
using System.Collections.Generic;

namespace AdaptableMapper
{
    public sealed class MappingScopeRoot : MappingScope
    {
        public List<MappingScopeComposite> MappingScopeComposites { get; set; }

        public MappingScopeRoot(
            List<MappingScopeComposite> mappingScopeComposite)
        {
            MappingScopeComposites = mappingScopeComposite;
        }

        void MappingScope.Traverse(Context context)
        {
            foreach (MappingScopeComposite child in MappingScopeComposites)
                child.Traverse(context);
        }
    }
}