using System;
using MappingFramework.Configuration;
using MappingFramework.Converters;
using MappingFramework.Process;
using MappingFramework.Traversals;
using MappingFramework.Visitors;

namespace MappingFramework.Compositions
{
    public class GetValueTraversalDaysBetweenDates : GetValueTraversal, ResolvableByTypeId, IVisitable
    {
        public const string _typeId = "37ef235d-affc-40b2-8436-15bc73e83101";
        public string TypeId => _typeId;

        public GetValueTraversalDaysBetweenDates() { }
        public GetValueTraversalDaysBetweenDates(GetValueTraversal getValueTraversalA, GetValueTraversal getValueTraversalB)
        {
            GetValueTraversalA = getValueTraversalA;
            GetValueTraversalB = getValueTraversalB;
        }

        public GetValueTraversal GetValueTraversalA { get; set; }
        public GetValueTraversal GetValueTraversalB { get; set; }
        public bool IncludeLastDay { get; set; }

        public string GetValue(Context context)
        {
            string valueA = GetValueTraversalA.GetValue(context);
            string valueB = GetValueTraversalB.GetValue(context);

            if (!DateTime.TryParse(valueA, out DateTime valueADateTime))
            {
                context.AddInformation($"Result: {valueA}, of {nameof(GetValueTraversalA)} is not a valid date", InformationType.Warning);
                return string.Empty;
            }

            if (!DateTime.TryParse(valueB, out DateTime valueBDateTime))
            {
                context.AddInformation($"Result: {valueB}, of {nameof(GetValueTraversalB)} is not a valid date", InformationType.Warning);
                return string.Empty;
            }

            int includeLastDay = IncludeLastDay ? 1 : 0;
            string result = (Math.Abs(valueADateTime.Subtract(valueBDateTime).Days) + includeLastDay).ToString();
            return result;
        }

        void IVisitable.Receive(IVisitor visitor)
        {
            visitor.Visit(GetValueTraversalA);
            visitor.Visit(GetValueTraversalB);
        }
    }
}