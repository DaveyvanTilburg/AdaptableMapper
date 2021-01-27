using MappingFramework.Traversals;
using System.Collections.Generic;
using MappingFramework.Conditions;
using MappingFramework.Visitors;

namespace MappingFramework.Configuration
{
    public sealed class MappingScopeComposite : IVisitable
    {
        public GetListValueTraversal GetListValueTraversal { get; set; }
        public Condition Condition { get; set; }

        public GetTemplateTraversal GetTemplateTraversal { get; set; }
        public ChildCreator ChildCreator { get; set; }

        public List<Mapping> Mappings { get; set; }
        public List<MappingScopeComposite> MappingScopeComposites { get; set; }

        public MappingScopeComposite() { }

        public MappingScopeComposite(
            List<MappingScopeComposite> mappingScopeComposites,
            List<Mapping> mappings,
            Condition condition,
            GetListValueTraversal getListValueTraversal,
            GetTemplateTraversal getTemplateTraversal,
            ChildCreator childCreator)
        {
            MappingScopeComposites = new List<MappingScopeComposite>(mappingScopeComposites ?? new List<MappingScopeComposite>());
            Mappings = new List<Mapping>(mappings ?? new List<Mapping>());
            Condition = condition;
            GetListValueTraversal = getListValueTraversal;
            GetTemplateTraversal = getTemplateTraversal;
            ChildCreator = childCreator;
        }

        public void Traverse(Context context)
        {
            MethodResult<IEnumerable<object>> scopes = GetListValueTraversal.GetValues(context);
            if (!scopes.IsValid)
                return;

            Template template = GetTemplateTraversal.GetTemplate(context, context.Target);

            foreach (object scope in scopes.Value)
            {
                object newTargetChild = ChildCreator.CreateChild(context, template);
                Context childContext = context.Copy(scope, newTargetChild);

                if (Condition != null && !Condition.Validate(childContext))
                    continue;

                ChildCreator.AddToParent(context, template, newTargetChild);
                TraverseChild(childContext);
            }
        }

        private void TraverseChild(Context context)
        {
            foreach (MappingScopeComposite mappingScopeComposite in MappingScopeComposites)
                mappingScopeComposite.Traverse(context);

            foreach (Mapping mapping in Mappings)
                mapping.Map(context);
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(GetListValueTraversal);
            visitor.Visit(Condition);
            visitor.Visit(GetTemplateTraversal);
            visitor.Visit(ChildCreator);

            foreach (Mapping mapping in Mappings)
                visitor.Visit(mapping);

            foreach (MappingScopeComposite scope in MappingScopeComposites)
                visitor.Visit(scope);
        }
    }
}