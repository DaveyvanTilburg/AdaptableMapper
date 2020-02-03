using System.Collections.Generic;
using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Compositions
{
    public class GetConcatenatedValueTraversal : GetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "0c64cc9e-4273-4440-b7b0-ddf42db8a8fb";
        public string TypeId => _typeId;

        public GetConcatenatedValueTraversal()
            => GetValueTraversals = new List<GetValueTraversal>();

        public GetConcatenatedValueTraversal(List<GetValueTraversal> getListValueTraversals, string separator)
        {
            GetValueTraversals = getListValueTraversals;
            Separator = separator;
        }

        public List<GetValueTraversal> GetValueTraversals { get; set; }
        public string Separator { get; set; }


        public string GetValue(Context context)
        {
            if (!Validate())
                return string.Empty;

            var resultParts = new List<string>();
            foreach (GetValueTraversal getValueTraversal in GetValueTraversals)
            {
                resultParts.Add(getValueTraversal.GetValue(context));
            }

            string result = string.Join(Separator, resultParts);
            return result;
        }

        private bool Validate()
        {
            bool result = true;

            if (GetValueTraversals == null || GetValueTraversals.Count == 0)
            {
                Process.ProcessObservable.GetInstance().Raise($"GetConcatenatedValueTraversal#1; {nameof(GetValueTraversals)} cannot be null", "error");
                result = false;
            }

            if (Separator == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"GetConcatenatedValueTraversal#2; {nameof(Separator)} cannot be null", "error");
                result = false;
            }

            return result;
        }
    }
}