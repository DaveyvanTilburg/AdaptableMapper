using System.Collections.Generic;
using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Compositions
{
    public sealed class GetListSearchValueTraversal : GetListValueTraversal, ResolvableByTypeId
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
            if (!Validate())
                return new NullMethodResult<IEnumerable<object>>();

            GetValueTraversalPathProperty pathProperty = GetListValueTraversal as GetValueTraversalPathProperty;

            string searchValue = GetValueTraversal.GetValue(context);

            string tempPath = pathProperty.Path;
            string actualPath = pathProperty.Path.Replace("{{searchValue}}", searchValue);
            pathProperty.Path = actualPath;

            MethodResult<IEnumerable<object>> result = GetListValueTraversal.GetValues(context);
            pathProperty.Path = tempPath;

            return result;
        }

        private bool Validate()
        {
            bool result = true;

            if (GetListValueTraversal == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"GetListSearchValueTraversal#1; {nameof(GetListValueTraversal)} cannot be null", "error");
                result = false;
            }

            if (GetValueTraversal == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"GetListSearchValueTraversal#2; {nameof(GetValueTraversal)} cannot be null", "error");
                result = false;
            }

            return result;
        }
    }
}