using AdaptableMapper.Contexts;
using AdaptableMapper.Traversals;
using System.Collections.Generic;

namespace AdaptableMapper
{
    public sealed class MappingScopeComposite : MappingScope
    {
        public List<MappingScopeComposite> MappingScopeComposites { get; set; }
        public List<Mapping> Mappings { get; set; }
        public GetScopeTraversal GetScopeTraversal { get; set; }

        public Traversal Traversal { get; set; }
        public TraversalToGetTemplate TraversalToGetTemplate { get; set; }
        public ChildCreator ChildCreator { get; set; }

        public MappingScopeComposite(
            List<MappingScopeComposite> mappingScopeComposites, 
            List<Mapping> mappings, 
            GetScopeTraversal getScopeTraversal, 
            Traversal traversal, 
            TraversalToGetTemplate traversalToGetTemplate, 
            ChildCreator childCreator)
        {
            MappingScopeComposites = mappingScopeComposites;
            Mappings = mappings;
            GetScopeTraversal = getScopeTraversal;
            Traversal = traversal;
            TraversalToGetTemplate = traversalToGetTemplate;
            ChildCreator = childCreator;
        }

        public void Traverse(Context context)
        {
            if (!Validate())
                return;

            IEnumerable<object> scope = GetScopeTraversal.GetScope(context.Source);

            object parent = Traversal.Traverse(context.Target);
            object template = TraversalToGetTemplate.Traverse(parent);

            foreach (object item in scope)
            {
                object newChild = ChildCreator.CreateChildOn(parent, template);

                Context childContext = new Context(source:item, target:newChild);
                TraverseChild(childContext);
            }
        }

        private bool Validate()
        {
            bool result = true;

            if (GetScopeTraversal == null)
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#7 GetScopeTraversal cannot be null", "error");
                result = false;
            }

            if (Traversal == null)
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#8 Traversal cannot be null", "error");
                result = false;
            }

            if (TraversalToGetTemplate == null)
            {
                Process.ProcessObservable.GetInstance().Raise("TREE#9; TraversalToGetTemplate cannot be null", "error");
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