using System.Collections.Generic;
using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.Traversals;
using MappingFramework.Visitors;

namespace MappingFramework.Compositions
{
    public sealed class GetListSearchValueTraversal : GetListValueTraversal, ResolvableByTypeId, IVisitable
    {
        public const string _typeId = "692a9cb3-61e6-4e99-a698-1e537b22be19";
        public string TypeId => _typeId;

        public GetListSearchValueTraversal() { }
        public GetListSearchValueTraversal(GetListValueTraversal getListValueTraversal, GetValueTraversal getValueTraversal)
        {
            GetListValueTraversal = getListValueTraversal;
            GetValueTraversal = getValueTraversal;
        }

        public GetListValueTraversal GetListValueTraversal { get; set; }
        public GetValueTraversal GetValueTraversal { get; set; }

        public MethodResult<IEnumerable<object>> GetValues(Context context)
        {
            GetValueTraversalPathProperty pathProperty = GetListValueTraversal as GetValueTraversalPathProperty;

            string searchValue = GetValueTraversal.GetValue(context);

            string tempPath = pathProperty.Path;
            string actualPath = pathProperty.Path.Replace("{{searchValue}}", searchValue);
            pathProperty.Path = actualPath;

            MethodResult<IEnumerable<object>> result = GetListValueTraversal.GetValues(context);
            pathProperty.Path = tempPath;

            return result;
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(GetListValueTraversal);
            visitor.Visit(GetValueTraversal);
        }
    }
}