using System.Collections.Generic;
using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.Traversals;
using MappingFramework.Visitors;

namespace MappingFramework.Compositions
{
    public class GetConcatenatedValueTraversal : GetValueTraversal, ResolvableByTypeId, IVisitable
    {
        public const string _typeId = "0c64cc9e-4273-4440-b7b0-ddf42db8a8fb";
        public string TypeId => _typeId;

        public GetConcatenatedValueTraversal()
            => ListOfGetValueTraversal = new List<GetValueTraversal>();

        public GetConcatenatedValueTraversal(List<GetValueTraversal> listOfGetValueTraversals)
            => ListOfGetValueTraversal = new List<GetValueTraversal>(listOfGetValueTraversals ?? new List<GetValueTraversal>());
        public GetConcatenatedValueTraversal(List<GetValueTraversal> listOfGetValueTraversals, string separator)
        {
            ListOfGetValueTraversal = new List<GetValueTraversal>(listOfGetValueTraversals ?? new List<GetValueTraversal>());
            Separator = separator;
        }

        public List<GetValueTraversal> ListOfGetValueTraversal { get; set; }
        public string Separator { get; set; }


        public string GetValue(Context context)
        {
            var resultParts = new List<string>();
            foreach (GetValueTraversal getValueTraversal in ListOfGetValueTraversal)
            {
                string value = getValueTraversal.GetValue(context);

                if (!string.IsNullOrEmpty(value))
                    resultParts.Add(value);
            }

            string result = string.Join(Separator ?? string.Empty, resultParts);
            return result;
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            foreach (GetValueTraversal getValueTraversal in ListOfGetValueTraversal)
                visitor.Visit(getValueTraversal);
        }
    }
}