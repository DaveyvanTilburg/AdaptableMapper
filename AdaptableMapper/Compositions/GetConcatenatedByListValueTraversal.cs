﻿using System.Collections.Generic;
using AdaptableMapper.Configuration;
using AdaptableMapper.Converters;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Compositions
{
    public class GetConcatenatedByListValueTraversal : GetValueTraversal, ResolvableByTypeId
    {
        public const string _typeId = "292d79b5-e5f6-4029-b4d6-413e0a093d87";
        public string TypeId => _typeId;

        public GetConcatenatedByListValueTraversal() { }
        public GetConcatenatedByListValueTraversal(GetListValueTraversal getListValueTraversal, GetValueTraversal getValueTraversal, string separator)
        {
            GetListValueTraversal = getListValueTraversal;
            GetValueTraversal = getValueTraversal;
            Separator = separator;
        }

        public GetListValueTraversal GetListValueTraversal { get; set; }
        public GetValueTraversal GetValueTraversal { get; set; }
        public string Separator { get; set; }


        public string GetValue(Context context)
        {
            if (!Validate())
                return string.Empty;

            MethodResult<IEnumerable<object>> values = GetListValueTraversal.GetValues(context);

            var resultParts = new List<string>();
            foreach (object value in values.Value)
            {
                string resultPart = GetValueTraversal.GetValue(new Context(value, context.Target));
                resultParts.Add(resultPart);
            }

            string result = string.Join(Separator, resultParts);
            return result;
        }

        private bool Validate()
        {
            bool result = true;

            if (GetListValueTraversal == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"GetConcatenatedByListValueTraversal#1; {nameof(GetListValueTraversal)} cannot be null", "error");
                result = false;
            }

            if (GetValueTraversal == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"GetConcatenatedByListValueTraversal#2; {nameof(GetValueTraversal)} cannot be null", "error");
                result = false;
            }

            if (Separator == null)
            {
                Process.ProcessObservable.GetInstance().Raise($"GetConcatenatedByListValueTraversal#3; {nameof(Separator)} cannot be null", "error");
                result = false;
            }

            return result;
        }
    }
}