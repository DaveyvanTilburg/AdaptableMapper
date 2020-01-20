using System.Collections.Generic;
using System.Linq;
using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Compositions
{
    public class GetNumberOfHits : GetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "5f9d1ca5-95a8-4a07-aa44-4ea6e8ec8fa9";
        public string TypeId => _typeId;

        public GetNumberOfHits()
            => GetListValueTraversals = new List<GetListValueTraversal>();

        public GetNumberOfHits(List<GetListValueTraversal> getListValueTraversals)
            => GetListValueTraversals = getListValueTraversals;

        public List<GetListValueTraversal> GetListValueTraversals { get; set; }

        public string GetValue(Context context)
        {
            var objects = new List<object>();

            foreach (GetListValueTraversal getListValueTraversal in GetListValueTraversals)
            {
                MethodResult<IEnumerable<object>> getListValueTraversalObjects = getListValueTraversal.GetValues(context);
                if (getListValueTraversalObjects.IsValid)
                    objects.AddRange(getListValueTraversalObjects.Value);
            }

            int hits = objects.Count();
            return hits.ToString();
        }
    }
}