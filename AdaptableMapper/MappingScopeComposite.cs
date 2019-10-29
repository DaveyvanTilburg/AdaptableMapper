using AdaptableMapper.Contexts;
using AdaptableMapper.Traversals;
using System.Collections.Generic;

namespace AdaptableMapper
{
    public sealed class MappingScopeComposite
    {
        public List<MappingScopeComposite> Children { get; set; }
        public List<Mapping> Mappings { get; set; }
        public GetScopeTraversal GetScopeTraversion { get; set; }
        public Traversal TemplateParentTraversal { get; set; }
        public TraversalTemplate TemplateTraversal { get; set; }
        public CreateNewChild CreateNewChild { get; set; }

        public MappingScopeComposite(
            List<MappingScopeComposite> children, 
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

        internal void Traverse(Context context)
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

            foreach(MappingScopeComposite child in Children)
                child.Traverse(context);
        }
    }
}