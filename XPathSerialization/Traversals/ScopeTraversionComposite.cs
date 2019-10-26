using System.Collections.Generic;

namespace XPathSerialization.Traversals
{
    public abstract class ScopeTraversionComposite
    {
        public List<ScopeTraversionComposite> Children { get; set; }
        public List<Map> Mappings { get; set; }
        public GetScopeTraversal GetScopeTraversion { get; set; }
        public Traversal Traversal { get; set; }
        public CreateNewChild CreateNewChild { get; set; }

        public void Traverse(Context context)
        {
            IEnumerable<object> scope = GetScopeTraversion.GetScope(context.Source);
            object parent = Traversal.Traverse(context.Target);

            foreach (object item in scope)
            {
                object newChild = CreateNewChild.CreateChildOn(parent);

                Context childContext = new Context() { Source = item, Target = newChild };
                TraverseChild(childContext);
            }
        }

        private void TraverseChild(Context context)
        {
            foreach(Map map in Mappings)
            {
                string value = map.GetTraversion.GetValue(context.Source);

                map.SetTraversion.SetValue(context.Target, value);
            }

            foreach(ScopeTraversionComposite child in Children)
                child.Traverse(context);
        }
    }
}