using MappingFramework.Conditions;
using MappingFramework.Configuration;
using MappingFramework.Traversals;
using MappingFramework.ValueMutations;

namespace MappingFramework.Visitors
{
    internal interface IVisitor
    {
        void Visit(MappingConfiguration mappingConfiguration);
        void Visit(ContextFactory contextFactory);
        void Visit(ObjectConverter objectConverter);
        void Visit(TargetInstantiator targetInstantiator);
        void Visit(ResultObjectConverter resultObjectConverter);
        void Visit(Mapping mapping);
        void Visit(GetValueTraversal getValueTraversal);
        void Visit(GetListValueTraversal getListValueTraversal);
        void Visit(SetValueTraversal setValueTraversal);
        void Visit(Condition condition);
        void Visit(ValueMutation valueMutation);
        void Visit(GetTemplateTraversal getTemplateTraversal);
        void Visit(ChildCreator childCreator);
        void Visit(MappingScopeComposite mappingScopeComposite);
    }
}