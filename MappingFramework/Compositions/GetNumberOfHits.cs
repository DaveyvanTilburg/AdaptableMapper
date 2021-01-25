using System.Collections.Generic;
using MappingFramework.Configuration;
using MappingFramework.ContentTypes;
using MappingFramework.Converters;
using MappingFramework.Traversals;
using MappingFramework.Visitors;

namespace MappingFramework.Compositions
{
    [ContentType(ContentType.Any)]
    public class GetNumberOfHits : GetValueTraversal, ResolvableByTypeId, IVisitable
    {
        public const string _typeId = "5f9d1ca5-95a8-4a07-aa44-4ea6e8ec8fa9";
        public string TypeId => _typeId;

        public GetNumberOfHits()
            => ListOfGetListValueTraversal = new List<GetListValueTraversal>();

        public GetNumberOfHits(List<GetListValueTraversal> getListValueTraversals)
            => ListOfGetListValueTraversal = new List<GetListValueTraversal>(getListValueTraversals ?? new List<GetListValueTraversal>());

        public List<GetListValueTraversal> ListOfGetListValueTraversal { get; set; }

        public string GetValue(Context context)
        {
            var objects = new List<object>();

            foreach (GetListValueTraversal getListValueTraversal in ListOfGetListValueTraversal)
            {
                MethodResult<IEnumerable<object>> getListValueTraversalObjects = getListValueTraversal.GetValues(context);
                if (getListValueTraversalObjects.IsValid)
                    objects.AddRange(getListValueTraversalObjects.Value);
            }

            int hits = objects.Count;
            return hits.ToString();
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            foreach (GetListValueTraversal getListValueTraversal in ListOfGetListValueTraversal)
                visitor.Visit(getListValueTraversal);
        }
    }
}