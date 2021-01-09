using System;
using System.Collections.Generic;
using MappingFramework.Compositions;
using MappingFramework.Conditions;
using MappingFramework.Traversals;
using MappingFramework.Traversals.Dictionary;
using MappingFramework.Traversals.Json;
using MappingFramework.Traversals.Model;
using MappingFramework.Traversals.Xml;
using MappingFramework.ValueMutations;

namespace MappingFramework
{
    public static class OptionLists
    {
        private static List<GetValueTraversal> ComposedGetValueTraversals => new List<GetValueTraversal>
        {
            new GetAdditionalSourceValue(),
            new GetConcatenatedByListValueTraversal(),
            new GetConcatenatedValueTraversal(),
            new GetMutatedValueTraversal(),
            new NullGetValueTraversal(),
            new GetNumberOfHits(),
            new GetSearchValueTraversal(),
            new GetStaticValue(),
            new GetValueTraversalDaysBetweenDates(),
            new IfConditionThenAElseBGetValueTraversal()
        };

        private static List<GetValueTraversal> NewXmlGetValueTraversals => new List<GetValueTraversal>
        {
            new XmlGetValueTraversal(),
            new XmlGetThisValueTraversal()
        };

        private static List<GetValueTraversal> NewJsonGetValueTraversals => new List<GetValueTraversal>
        {
            new JsonGetValueTraversal()
        };

        private static List<GetValueTraversal> NewDataStructureGetValueTraversals => new List<GetValueTraversal>
        {
            new ModelGetValueTraversal()
        };

        private static List<GetValueTraversal> XmlGetValueTraversals() => CreateList(NewXmlGetValueTraversals, ComposedGetValueTraversals);
        private static List<GetValueTraversal> JsonGetValueTraversals() => CreateList(NewJsonGetValueTraversals, ComposedGetValueTraversals);
        private static List<GetValueTraversal> DataStructureGetValueTraversals() => CreateList(NewDataStructureGetValueTraversals, ComposedGetValueTraversals);
        
        public static List<GetValueTraversal> GetValueTraversals(ContentType contentType)
        {
            switch(contentType)
            {
                case ContentType.Xml:
                    return XmlGetValueTraversals();
                case ContentType.Json:
                    return JsonGetValueTraversals();
                case ContentType.DataStructure:
                    return DataStructureGetValueTraversals();
                default:
                    throw new Exception($"{contentType} is unsupported for {nameof(GetValueTraversal)}");
            }
        }



        private static List<SetValueTraversal> ComposedSetValueTraversals => new List<SetValueTraversal>
        {
            new SetMutatedValueTraversal()
        };

        private static List<SetValueTraversal> NewXmlSetValueTraversals => new List<SetValueTraversal>
        {
            new XmlSetValueTraversal(),
            new XmlSetThisValueTraversal(),
            new XmlSetGeneratedIdValueTraversal()
        };

        private static List<SetValueTraversal> NewJsonSetValueTraversals => new List<SetValueTraversal>
        {
            new JsonSetValueTraversal()
        };

        private static List<SetValueTraversal> NewDataStructureSetValueTraversals => new List<SetValueTraversal>
        {
            new ModelSetValueOnPathTraversal(),
            new ModelSetValueOnPropertyTraversal()
        };

        private static List<SetValueTraversal> NewDictionarySetValueTraversals => new List<SetValueTraversal>
        {
            new DictionarySetValueTraversal()
        };

        private static List<SetValueTraversal> XmlSetValueTraversals() => CreateList(NewXmlSetValueTraversals, ComposedSetValueTraversals);
        private static List<SetValueTraversal> JsonSetValueTraversals() => CreateList(NewJsonSetValueTraversals, ComposedSetValueTraversals);
        private static List<SetValueTraversal> DataStructureSetValueTraversals() => CreateList(NewDataStructureSetValueTraversals, ComposedSetValueTraversals);
        private static List<SetValueTraversal> DictionaryGetValueTraversals() => CreateList(NewDictionarySetValueTraversals, ComposedSetValueTraversals);

        public static List<SetValueTraversal> SetValueTraversals(ContentType contentType)
        {
            switch (contentType)
            {
                case ContentType.Xml:
                    return XmlSetValueTraversals();
                case ContentType.Json:
                    return JsonSetValueTraversals();
                case ContentType.DataStructure:
                    return DataStructureSetValueTraversals();
                case ContentType.Dictionary:
                    return DictionaryGetValueTraversals();
                default:
                    throw new Exception($"{contentType} is unsupported for {nameof(SetValueTraversal)}");
            }
        }

        public static List<ValueMutation> ValueMutations() => new List<ValueMutation>
        {
            new CreateSeparatedRangeFromNumberValueMutation(),
            new DateValueMutation(),
            new DictionaryReplaceValueMutation(),
            new ListOfValueMutations(),
            new NumberValueMutation(),
            new PlaceholderValueMutation(),
            new ReplaceValueMutation(),
            new SubstringValueMutation(),
            new ToLowerValueMutation(),
            new ToUpperValueMutation(),
            new TrimValueMutation()
        };

        public static List<Condition> Conditions() => new List<Condition>
        {
            new CompareCondition(),
            new ListOfConditions(),
            new NotEmptyCondition()
        };

        private static List<T> CreateList<T>(params IEnumerable<T>[] itemsList)
        {
            var result = new List<T>();
            
            foreach (IEnumerable<T> items in itemsList)
                result.AddRange(items);

            return result;
        }
    }
}