using System.Collections.Generic;
using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.Traversals;
using MappingFramework.Visitors;

namespace MappingFramework.Compositions
{
    public class GetConcatenatedByListValueTraversal : GetValueTraversal, ResolvableByTypeId, IVisitable
    {
        public const string _typeId = "292d79b5-e5f6-4029-b4d6-413e0a093d87";
        public string TypeId => _typeId;

        public GetConcatenatedByListValueTraversal() { }
        public GetConcatenatedByListValueTraversal(GetListValueTraversal getListValueTraversal, GetValueTraversal getValueTraversal)
        {
            GetListValueTraversal = getListValueTraversal;
            GetValueTraversal = getValueTraversal;
        }
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
            MethodResult<IEnumerable<object>> values = GetListValueTraversal.GetValues(context);

            var resultParts = new List<string>();
            foreach (object value in values.Value)
            {
                string resultPart = GetValueTraversal.GetValue(new Context(value, context.Target, context.AdditionalSourceValues));
                resultParts.Add(resultPart);
            }

            string result = string.Join(Separator ?? string.Empty, resultParts);
            return result;
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(GetListValueTraversal);
            visitor.Visit(GetValueTraversal);
        }
    }
}