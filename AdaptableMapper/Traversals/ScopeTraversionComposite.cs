using System.Collections.Generic;

namespace AdaptableMapper.Traversals
{
    public sealed class ScopeTraversionComposite
    {
        public List<ScopeTraversionComposite> Children { get; set; }
        public List<Mapping> Mappings { get; set; }
        public GetScopeTraversal GetScopeTraversion { get; set; }
        public Traversal TemplateParentTraversal { get; set; }
        public TraversalTemplate TemplateTraversal { get; set; }
        public CreateNewChild CreateNewChild { get; set; }

        public ScopeTraversionComposite(
            List<ScopeTraversionComposite> children, 
            List<Mapping> mappings, 
            GetScopeTraversal getScopeTraversion, 
            Traversal templateParentTraversal, 
            TraversalTemplate templateTraversal, 
            CreateNewChild createNewChild)
        {
            Children = children;
            Mappings = mappings;
            GetScopeTraversion = getScopeTraversion;
            TemplateParentTraversal = templateParentTraversal;
            TemplateTraversal = templateTraversal;
            CreateNewChild = createNewChild;
        }

        public void Traverse(Context context)
        {
            IEnumerable<object> scope = GetScopeTraversion.GetScope(context.Source);

            object parent = TemplateParentTraversal.Traverse(context.Target);
            object template = TemplateTraversal.Traverse(parent);

            foreach (object item in scope)
            {
                object newChild = CreateNewChild.CreateChildOn(parent, template);

                Context childContext = new Context(source:item, target:newChild);
                TraverseChild(childContext);
            }
        }

        private void TraverseChild(Context context)
        {
            foreach(Mapping mapping in Mappings)
                mapping.Map(context);

            foreach(ScopeTraversionComposite child in Children)
                child.Traverse(context);
        }
    }
}