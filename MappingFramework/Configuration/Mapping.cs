using MappingFramework.Traversals;
using MappingFramework.Visitors;

namespace MappingFramework.Configuration
{
    public sealed class Mapping : IVisitable
    {
        public GetValueTraversal GetValueTraversal { get; set; }
        public SetValueTraversal SetValueTraversal { get; set; }

        public Mapping(){}
        
        public Mapping(
            GetValueTraversal getValueTraversal, 
            SetValueTraversal setValueTraversal)
        {
            GetValueTraversal = getValueTraversal;
            SetValueTraversal = setValueTraversal;
        }

        public void Map(Context context)
        {
            string value = GetValueTraversal.GetValue(context);

            SetValueTraversal.SetValue(context, value);
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(GetValueTraversal);
            visitor.Visit(SetValueTraversal);
        }
    }
}