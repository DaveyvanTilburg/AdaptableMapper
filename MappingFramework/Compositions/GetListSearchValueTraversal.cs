using System.Collections.Generic;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;
using MappingFramework.Visitors;

namespace MappingFramework.Compositions
{
    [ContentType(ContentType.Any)]
    public sealed class GetListSearchValueTraversal : GetListValueTraversal, ResolvableByTypeId, IVisitable
    {
        public const string _typeId = "692a9cb3-61e6-4e99-a698-1e537b22be19";
        public string TypeId => _typeId;

        public GetListSearchValueTraversal() { }
        public GetListSearchValueTraversal(GetListSearchPathValueTraversal getListValueTraversal, GetValueTraversal getValueTraversal)
        {
            GetListValueTraversal = getListValueTraversal;
            GetValueTraversal = getValueTraversal;
        }

        public GetListSearchPathValueTraversal GetListValueTraversal { get; set; }
        public GetValueTraversal GetValueTraversal { get; set; }

        public MethodResult<IEnumerable<object>> GetValues(Context context)
        {
            string searchValue = GetValueTraversal.GetValue(context);

            string tempPath = GetListValueTraversal.Path;
            string actualPath = GetListValueTraversal.Path.Replace("{{searchValue}}", searchValue);
            GetListValueTraversal.Path = actualPath;

            MethodResult<IEnumerable<object>> result = GetListValueTraversal.GetValues(context);
            GetListValueTraversal.Path = tempPath;

            return result;
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(GetListValueTraversal);
            visitor.Visit(GetValueTraversal);
        }
    }
}