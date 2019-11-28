using AdaptableMapper.Contexts;
using AdaptableMapper.Traversals;
using System.Collections.Generic;

namespace AdaptableMapper
{
    public sealed class MappingScopeComposite
    {
        public List<MappingScopeComposite> MappingScopeComposites { get; set; }
        public List<Mapping> Mappings { get; set; }
        public GetScopeTraversal GetScopeTraversal { get; set; }

        public GetTemplate GetTemplate { get; set; }
        public ChildCreator ChildCreator { get; set; }

        public MappingScopeComposite(
            List<MappingScopeComposite> mappingScopeComposites, 
            List<Mapping> mappings, 
            GetScopeTraversal getScopeTraversal, 
            GetTemplate getTemplate, 
            ChildCreator childCreator)
        {
            MappingScopeComposites = mappingScopeComposites;
            Mappings = mappings;
            GetScopeTraversal = getScopeTraversal;
            GetTemplate = getTemplate;
            ChildCreator = childCreator;
        }

        public void Traverse(Context context)
        {
            if (!Validate())
                return;

            IEnumerable<object> scope = GetScopeTraversal.GetScope(context.Source);

            Template template = GetTemplate.Get(context.Target);

            foreach (object item in scope)
            {
                object newChild = ChildCreator.CreateChild(template);

                Context childContext = new Context(source:item, target:newChild);
                TraverseChild(childContext);
            }
        }

        private bool Validate()
        {
            bool result = true;

            if (GetScopeTraversal == null)
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#7; GetScopeTraversal cannot be null", "error");
                result = false;
            }

            if (GetTemplate == null)
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#9; Get cannot be null", "error");
                result = false;
            }

            if (ChildCreator == null)
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#10; ChildCreator cannot be null", "error");
                result = false;
            }

            return result;
        }

        private void TraverseChild(Context context)
        {
            foreach(Mapping mapping in Mappings)
                mapping.Map(context);

            foreach(MappingScopeComposite mappingScopeComposite in MappingScopeComposites)
                mappingScopeComposite.Traverse(context);
        }
    }
}