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

        public void Traverse(Context context, MappingCaches mappingCaches)
        {
            MethodResult<IEnumerable<object>> scope = GetListValueTraversal.GetValues(context);
            if (!scope.IsValid)
                return;

            Template template = GetTemplateTraversal.GetTemplate(context, context.Target, mappingCaches);

            foreach (object item in scope.Value)
            {
                object newChild = ChildCreator.CreateChild(context, template);
                Context childContext = new Context(source: item, target: newChild, context.AdditionalSourceValues);

                if (Condition != null && !Condition.Validate(childContext))
                    continue;

                ChildCreator.AddToParent(context, template, newChild);
                TraverseChild(childContext, mappingCaches);
            }
        }

        private void TraverseChild(Context context, MappingCaches mappingCaches)
        {
            foreach (MappingScopeComposite mappingScopeComposite in MappingScopeComposites)
                mappingScopeComposite.Traverse(context, mappingCaches);

            foreach (Mapping mapping in Mappings)
                mapping.Map(context, mappingCaches);
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