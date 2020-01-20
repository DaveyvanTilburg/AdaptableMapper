using AdaptableMapper.Traversals;
using System.Collections.Generic;
using AdaptableMapper.Conditions;

namespace AdaptableMapper.Configuration
{
    public sealed class MappingScopeComposite
    {
        public List<MappingScopeComposite> MappingScopeComposites { get; set; }
        public List<Mapping> Mappings { get; set; }

        public GetListValueTraversal GetListValueTraversal { get; set; }
        public Condition Condition { get; set; }

        public GetTemplateTraversal GetTemplateTraversal { get; set; }
        public ChildCreator ChildCreator { get; set; }

        public MappingScopeComposite(
            List<MappingScopeComposite> mappingScopeComposites,
            List<Mapping> mappings,
            GetListValueTraversal getListValueTraversal,
            GetTemplateTraversal getTemplateTraversal,
            ChildCreator childCreator)
        {
            MappingScopeComposites = mappingScopeComposites;
            Mappings = mappings;
            GetListValueTraversal = getListValueTraversal;
            GetTemplateTraversal = getTemplateTraversal;
            ChildCreator = childCreator;
        }

        public void Traverse(Context context, MappingCaches mappingCaches)
        {
            if (!Validate())
                return;

            MethodResult<IEnumerable<object>> scope = GetListValueTraversal.GetValues(context);
            if (!scope.IsValid)
                return;

            Template template = GetTemplateTraversal.GetTemplate(context.Target, mappingCaches);

            foreach (object item in scope.Value)
            {
                object newChild = ChildCreator.CreateChild(template);
                Context childContext = new Context(source: item, target: newChild);

                if (Condition != null && !Condition.Validate(childContext))
                    continue;

                ChildCreator.AddToParent(template, newChild);
                TraverseChild(childContext, mappingCaches);
            }
        }

        private bool Validate()
        {
            bool result = true;

            if (GetListValueTraversal == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"TREE#7; {nameof(GetListValueTraversal)} cannot be null", "error");
                result = false;
            }

            if (GetTemplateTraversal == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"TREE#9; {nameof(GetTemplateTraversal)} cannot be null", "error");
                result = false;
            }

            if (ChildCreator == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"TREE#10; {nameof(ChildCreator)} cannot be null", "error");
                result = false;
            }

            return result;
        }

        private void TraverseChild(Context context, MappingCaches mappingCaches)
        {
            foreach (MappingScopeComposite mappingScopeComposite in MappingScopeComposites)
                mappingScopeComposite.Traverse(context, mappingCaches);

            foreach (Mapping mapping in Mappings)
                mapping.Map(context, mappingCaches);
        }
    }
}